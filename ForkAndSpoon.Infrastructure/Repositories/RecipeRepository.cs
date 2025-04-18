﻿using AutoMapper;
using ForkAndSpoon.Application.Factories;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Infrastructure.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly ForkAndSpoonDbContext _context;
        private readonly IMapper _autoMapper;

        public RecipeRepository(ForkAndSpoonDbContext databaseContext, IMapper autoMapper)
        {
            _context = databaseContext;
            _autoMapper = autoMapper;
        }

        public async Task<List<RecipeReadDto>> GetAllRecipesAsync(string? categoryFilter = null, string? ingredientFilter = null, string? dietaryPreferenceFilter = null, string? sortOrder = null, int page = 1, int pageSize = 10)
        {
            var query = _context.Recipes
                .Where(recipe => !recipe.IsDeleted)
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
                .Include(recipe => recipe.RecipeIngredients)
                    .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeDietaryPreferences)
                    .ThenInclude(recipeDietaryPreference => recipeDietaryPreference.DietaryPreference)
                .FirstOrDefaultAsync(recipe => recipe.RecipeID == recipeId && !recipe.IsDeleted);

            if (matchingRecipe == null)
                return null;

            return _autoMapper.Map<RecipeReadDto>(matchingRecipe);
        }

        public async Task<RecipeReadDto> CreateRecipeAsync(RecipeCreateDto recipeCreateDto, int userId)
        {
            recipeCreateDto.CategoryID = await GetValidatedOrFallbackCategoryIdAsync(recipeCreateDto.CategoryID);

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

        private async Task<int> GetValidatedOrFallbackCategoryIdAsync(int? categoryId)
        {
            if (categoryId != null)
            {
                bool exists = await _context.Categories.AnyAsync(category => category.CategoryID == categoryId);
                if (exists) return categoryId.Value;
            }

            const int fallbackCategoryId = 1; // "Uncategorized"
            bool uncategorizedExists = await _context.Categories.AnyAsync(category => category.CategoryID == fallbackCategoryId);

            if (!uncategorizedExists)
                throw new ArgumentException("No valid category selected and fallback 'Uncategorized' category is missing.");

            return fallbackCategoryId;
        }

        public async Task<RecipeReadDto?> UpdateRecipeAsync(int recipeId, RecipeUpdateDto updatedRecipe, int userId, string role)
        {
            var recipeInDb = await _context.Recipes
                .Include(recipe => recipe.RecipeIngredients)
                    .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeDietaryPreferences)
                .FirstOrDefaultAsync(recipe => recipe.RecipeID == recipeId && (recipe.CreatedBy == userId || role == "Admin"));

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

        public async Task<bool> DeleteRecipeAsync(int recipeId, int userId, string role)
        {
            var recipeToDelete = await _context.Recipes.FirstOrDefaultAsync(recipe => recipe.RecipeID == recipeId);

            if (recipeToDelete == null || recipeToDelete.IsDeleted) return false;

            // Only allow if user is creator or admin
            if (recipeToDelete.CreatedBy != userId && role != "Admin")
                return false;

            recipeToDelete.IsDeleted = true;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<RecipeReadDto>> GetDeletedRecipesAsync()
        {
            var deletedRecipes = await _context.Recipes
                .Where(recipe => recipe.IsDeleted)
                .Select(recipe => new Recipe
                {
                    RecipeID = recipe.RecipeID,
                    Title = recipe.Title,
                    CreatedBy = recipe.CreatedBy,
                    Steps = recipe.Steps
                }).ToListAsync();

            return _autoMapper.Map<List<RecipeReadDto>>(deletedRecipes);
        }
        public async Task<RecipeReadDto?> GetDeletedRecipeByIdAsync(int recipeId)
        {
            var recipe = await _context.Recipes
                .Where(recipe => recipe.IsDeleted && recipe.RecipeID == recipeId)
                .Include(recipe => recipe.Category)
                .Include(recipe => recipe.RecipeIngredients).ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeDietaryPreferences).ThenInclude(recipeDietaryPref => recipeDietaryPref.DietaryPreference)
                .FirstOrDefaultAsync();

            if (recipe == null) return null;

            return _autoMapper.Map<RecipeReadDto>(recipe);
        }
        public async Task<bool> RestoreDeletedRecipeAsync(int recipeId)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(recipe => recipe.RecipeID == recipeId && recipe.IsDeleted);

            if (recipe == null) return false;

            recipe.IsDeleted = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}