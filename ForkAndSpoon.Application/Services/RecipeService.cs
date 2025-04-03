using AutoMapper;
using ForkAndSpoon.Application.DTOs.Recipe;
using ForkAndSpoon.Application.Factories;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;

namespace ForkAndSpoon.Application.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly ForkAndSpoonDbContext _context;
        private readonly IMapper _autoMapper;

        public RecipeService(ForkAndSpoonDbContext databaseContext, IMapper autoMapper)
        {
            _context = databaseContext;
            _autoMapper = autoMapper;
        }

        public async Task<List<RecipeReadDto>> GetAllRecipesAsync(string? categoryFilter = null, string? ingredientFilter = null, string? dietaryPreferenceFilter = null, string? sortOrder = null, int page = 1, int pageSize = 10)
        {
            var query = _context.Recipes
                .Include(recipe => recipe.Category)
                .Include(recipe => recipe.RecipeIngredients)
                    .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeDietaryPreferences)
                     .ThenInclude(recipeDietaryPreference => recipeDietaryPreference.DietaryPreference)
                .AsQueryable();

            // Category filtering
            if (!string.IsNullOrWhiteSpace(categoryFilter))
            {
                query = query.Where(recipe => recipe.Category != null && recipe.Category.Name.ToLower() == categoryFilter.ToLower());
            }

            // Ingredient filter
            if (!string.IsNullOrWhiteSpace(ingredientFilter))
            {
                query = query.Where(recipe => recipe.RecipeIngredients.Any(recipeIngredient => recipeIngredient.Ingredient.Name.ToLower() == ingredientFilter.ToLower()));
            }

            // Dietarypreference filter
            if (!string.IsNullOrWhiteSpace(dietaryPreferenceFilter))
            {
                query = query.Where(recipe => recipe.RecipeDietaryPreferences.Any(preference => preference.DietaryPreference.Name.ToLower() == dietaryPreferenceFilter.ToLower()));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortOrder))
            {
                switch (sortOrder.ToLower())
                {
                    case "title_desc":
                        query = query.OrderByDescending(recipe => recipe.Title);
                        break;
                    case "title_asc":
                        query = query.OrderBy(recipe => recipe.Title);
                        break;
                    default:
                        query = query.OrderBy(recipe => recipe.Title);
                        break;
                }
            }

            // Pagination
            int skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);

            var paginatedResult = await query.ToListAsync();

            return _autoMapper.Map<List<RecipeReadDto>>(paginatedResult);
        }

        public async Task<RecipeReadDto?> GetRecipeByIdAsync(int recipeId)
        {
            var matchingRecipe = await _context.Recipes
                .Include(recipe => recipe.Category)
                .FirstOrDefaultAsync(recipe => recipe.RecipeID == recipeId);

            if (matchingRecipe == null)
            {
                return null;
            }
            else
            {
                return _autoMapper.Map<RecipeReadDto>(matchingRecipe);
            }
        }

        public async Task<RecipeReadDto> CreateRecipeAsync(RecipeCreateDto recipeCreateDto, int userId)
        {
            // Validate category exists before anything else
            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryID == recipeCreateDto.CategoryID);
            if (!categoryExists)
            {
                throw new ArgumentException("Selected category does not exist.");
            }

            // Build recipe from factory
            var newRecipe = RecipeFactory.CreateRecipeFromDto(recipeCreateDto, userId);
            _context.Recipes.Add(newRecipe);

            // Prepare dietary preferences
            foreach (var preferenceName in recipeCreateDto.DietaryPreferences)
            {
                var existingPreference = await _context.DietaryPreferences
                    .FirstOrDefaultAsync(p => p.Name.ToLower() == preferenceName.ToLower());

                if (existingPreference == null)
                {
                    existingPreference = new DietaryPreference { Name = preferenceName };
                    _context.DietaryPreferences.Add(existingPreference);
                }

                newRecipe.RecipeDietaryPreferences.Add(new RecipeDietaryPreference
                {
                    Recipe = newRecipe,
                    DietaryPreference = existingPreference!
                });
            }

            await _context.SaveChangesAsync();

            // Reload with relations
            var recipeWithRelations = await _context.Recipes
                .Include(recipe => recipe.Category)
                .Include(recipe => recipe.RecipeIngredients)
                    .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeDietaryPreferences)
                    .ThenInclude(join => join.DietaryPreference)
                .FirstOrDefaultAsync(recipe => recipe.RecipeID == newRecipe.RecipeID);

            return _autoMapper.Map<RecipeReadDto>(recipeWithRelations);
        }

        public async Task<RecipeReadDto?> UpdateRecipeAsync(int recipeId, RecipeUpdateDto updatedRecipe, int userId)
        {
            var recipeInDb = await _context.Recipes
                .Include(recipe => recipe.RecipeIngredients)
                    .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeDietaryPreferences)
                .FirstOrDefaultAsync(recipe => recipe.RecipeID == recipeId && recipe.CreatedBy == userId);

            if (recipeInDb == null)
            {
                return null;
            }

            // Update basic properties
            recipeInDb.Title = updatedRecipe.Title;
            recipeInDb.Steps = updatedRecipe.Steps;
            recipeInDb.ImageUrl = updatedRecipe.ImageUrl;
            recipeInDb.CategoryID = updatedRecipe.CategoryID;

            // Clear existing dietary preferences
            _context.RecipeDietaryPreferences.RemoveRange(recipeInDb.RecipeDietaryPreferences);

            // Recreate dietary preference links
            foreach (var preferenceName in updatedRecipe.DietaryPreferences)
            {
                var existingPreference = await _context.DietaryPreferences
                    .FirstOrDefaultAsync(preference => preference.Name.ToLower() == preferenceName.ToLower());

                if (existingPreference == null)
                {
                    existingPreference = new DietaryPreference { Name = preferenceName };
                    _context.DietaryPreferences.Add(existingPreference);
                    await _context.SaveChangesAsync(); // So we get the ID
                }

                recipeInDb.RecipeDietaryPreferences.Add(new RecipeDietaryPreference
                {
                    RecipeID = recipeInDb.RecipeID,
                    DietaryPreferenceID = existingPreference.DietaryPreferenceID
                });
            }

            await _context.SaveChangesAsync();

            var updated = await _context.Recipes
                .Include(recipe => recipe.Category)
                .Include(recipe => recipe.RecipeIngredients).ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeDietaryPreferences).ThenInclude(join => join.DietaryPreference)
                .FirstOrDefaultAsync(recipe => recipe.RecipeID == recipeId);

            return _autoMapper.Map<RecipeReadDto>(updated);
        }
        public async Task<RecipeReadDto?> UpdateDietaryPreferencesAsync(int recipeId, int userId, RecipeDietaryPreferenceUpdateDto updateDto)
        {
            var recipe = await _context.Recipes
                .Include(recipe => recipe.RecipeDietaryPreferences)
                .FirstOrDefaultAsync(recipe => recipe.RecipeID == recipeId && recipe.CreatedBy == userId);

            if (recipe == null) return null;

            // Remove old preferences
            _context.RecipeDietaryPreferences.RemoveRange(recipe.RecipeDietaryPreferences);

            foreach (var preferenceName in updateDto.DietaryPreferences)
            {
                var preference = await _context.DietaryPreferences
                    .FirstOrDefaultAsync(preference => preference.Name.ToLower() == preferenceName.ToLower());

                if (preference == null)
                {
                    preference = new DietaryPreference { Name = preferenceName };
                    _context.DietaryPreferences.Add(preference);
                    await _context.SaveChangesAsync(); // So we get the ID
                }

                var link = new RecipeDietaryPreference
                {
                    RecipeID = recipeId,
                    DietaryPreferenceID = preference.DietaryPreferenceID
                };

                _context.RecipeDietaryPreferences.Add(link);
            }

            await _context.SaveChangesAsync();

            var updatedRecipe = await _context.Recipes
                .Include(recipe => recipe.Category)
                .Include(recipe => recipe.RecipeIngredients).ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeDietaryPreferences).ThenInclude(recipeDietaryPreference => recipeDietaryPreference.DietaryPreference)
                .FirstOrDefaultAsync(recipe => recipe.RecipeID == recipeId);

            return _autoMapper.Map<RecipeReadDto>(updatedRecipe);
        }

        public async Task<bool> DeleteRecipeAsync(int id)
        {
            var recipeToDelete = await _context.Recipes.FindAsync(id);

            if (recipeToDelete == null) return false;

            _context.Recipes.Remove(recipeToDelete);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
