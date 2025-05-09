using AutoMapper;
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


        public RecipeRepository(ForkAndSpoonDbContext context, IMapper autoMapper)
        {
            _context = context;
            _autoMapper = autoMapper;
        }
        public Task<IQueryable<Recipe>> GetAllRecipesQueryableAsync()
        {
            // Get all recipes with full related data, that are not soft-deleted
            var query = _context.Recipes
                .Where(recipe => !recipe.IsDeleted)
                .Include(recipe => recipe.Category)
                .Include(recipe => recipe.RecipeIngredients).ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeDietaryPreferences).ThenInclude(dietaryPreference => dietaryPreference.DietaryPreference)
                .AsQueryable();

            return Task.FromResult(query);
        }

        public async Task<OperationResult<RecipeReadDto>> GetRecipeByIdAsync(int recipeId)
        {
            try
            {
                // Find the recipe with full related data
                var recipe = await _context.Recipes
                    .Where(recipe => recipe.RecipeID == recipeId && !recipe.IsDeleted)
                        .Include(recipe => recipe.Category)
                        .Include(recipe => recipe.RecipeIngredients)
                            .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                        .Include(recipe => recipe.RecipeDietaryPreferences)
                            .ThenInclude(recipeDietaryPreference => recipeDietaryPreference.DietaryPreference)
                        .FirstOrDefaultAsync();

                // Return failure if not found
                if (recipe == null)
                    return OperationResult<RecipeReadDto>.Failure("Recipe not found.");

                // Map to DTO and return success
                var recipeDto = _autoMapper.Map<RecipeReadDto>(recipe);
                return OperationResult<RecipeReadDto>.Success(recipeDto);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<RecipeReadDto>.Failure($"Error fetching recipe: {ex.Message}");
            }
        }

        public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
        {
            // Create a new recipe and save it to the database
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        public async Task<Recipe?> GetRecipeOwnedByUserAsync(int recipeId, int userId)
        {
            // Get recipes owned by logged in user
            return await _context.Recipes
                .Include(recipe => recipe.RecipeDietaryPreferences)
                .FirstOrDefaultAsync(recipe => recipe.RecipeID == recipeId && recipe.CreatedBy == userId);
        }

        public async Task<Recipe?> GetRecipeEntityByIdAsync(int recipeId)
        {
            // Get a recipe by ID without checking for soft delete (used in delete/restore operations)
            return await _context.Recipes.FirstOrDefaultAsync(recipe => recipe.RecipeID == recipeId);
        }

        public async Task<List<Recipe>> GetDeletedRecipesAsync()
        {
            // Get all recipes that are soft-deleted
            return await _context.Recipes
                .Where(recipe => recipe.IsDeleted)
                .ToListAsync();
        }
        public async Task<Recipe?> GetDeletedRecipeByIdAsync(int recipeId)
        {
            // Get a soft-deleted recipe by ID with related entities (used for restoring)
            return await _context.Recipes
                .Where(recipe => recipe.IsDeleted && recipe.RecipeID == recipeId)
                .Include(recipe => recipe.Category)
                .Include(recipe => recipe.RecipeIngredients).ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeDietaryPreferences).ThenInclude(recipeDietaryPref => recipeDietaryPref.DietaryPreference)
                .FirstOrDefaultAsync();
        }
        public async Task<OperationResult<bool>> SaveChangesAsync()
        {
            try
            {
                // Save to database
                await _context.SaveChangesAsync();

                // Return success
                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<bool>.Failure($"Saving changes failed: {ex.Message}");
            }
        }
    }
}