using AutoMapper;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Application.Services
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly ForkAndSpoonDbContext _context;
        private readonly IMapper _autoMapper;

        public FavoriteRepository(ForkAndSpoonDbContext context, IMapper autoMapper)
        {
            _context = context;
            _autoMapper = autoMapper;
        }

        public async Task<OperationResult<string>> AddFavoriteAsync(int userId, int recipeId)
        {
            try
            {
                // Check if the recipe exists and is not soft-deleted
                var recipeExists = await _context.Recipes.AnyAsync(recipe => recipe.RecipeID == recipeId && !recipe.IsDeleted);

                if (!recipeExists)
                    return OperationResult<string>.Failure("Recipe does not exist or has been deleted.");

                // Check if the recipe is already in the user's favorites
                var alreadyFavorited = await _context.FavoriteRecipes.AnyAsync(favorite => favorite.UserID == userId && favorite.RecipeID == recipeId);

                if (alreadyFavorited)
                    return OperationResult<string>.Failure("Recipe already in favorites.");

                // Create and add the new favorite entry
                var favorite = new FavoriteRecipe
                {
                    UserID = userId,
                    RecipeID = recipeId
                };

                _context.FavoriteRecipes.Add(favorite);
                await _context.SaveChangesAsync();

                return OperationResult<string>.Success("Recipe added to favorites.");
            }
            catch (Exception ex) 
            {
                // Handle unexpected errors
                return OperationResult<string>.Failure($"Error adding favorite: {ex.Message}.");
            }
        }

        public async Task<OperationResult<List<RecipeReadDto>>> GetUserFavoritesAsync(int userId)
        {
            try
            {
                // Fetch the user's favorite recipes (excluding deleted ones), including full related data
                var favoriteRecipes = await _context.FavoriteRecipes
                .Where(favorite => favorite.UserID == userId && !favorite.Recipe.IsDeleted)
                .Include(favorite => favorite.Recipe)
                    .ThenInclude(recipe => recipe.Category)
                .Include(favorite => favorite.Recipe)
                    .ThenInclude(recipe => recipe.RecipeIngredients)
                        .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(favorite => favorite.Recipe)
                    .ThenInclude(recipe => recipe.RecipeDietaryPreferences)
                        .ThenInclude(recipeDietaryPreference => recipeDietaryPreference.DietaryPreference)
                .Select(favorite => favorite.Recipe)
                .ToListAsync();

                // Map to DTOs
                var recipeDtos = _autoMapper.Map<List<RecipeReadDto>>(favoriteRecipes);

                return OperationResult<List<RecipeReadDto>>.Success(recipeDtos);
            }
            catch (Exception ex) 
            {
                // Handle unexpected errors
                return OperationResult<List<RecipeReadDto>>.Failure($"Error fetching favorites: {ex.Message}.");
            }
        }

        public async Task<OperationResult<string>> RemoveFavoriteAsync(int userId, int recipeId)
        {
            try 
            {
                // Look up the favorite entry
                var favorite = await _context.FavoriteRecipes.FirstOrDefaultAsync(favoriteRecipe => favoriteRecipe.UserID == userId && favoriteRecipe.RecipeID == recipeId);

                if (favorite == null)
                    return OperationResult<string>.Failure("Favorite not found.");

                // Remove the favorite entry
                _context.FavoriteRecipes.Remove(favorite);
                await _context.SaveChangesAsync();

                return OperationResult<string>.Success("Recipe removed from favorites.");
            }
            
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<string>.Failure($"Failed to delete favorite: {ex.Message}.");
            }
        }
    }
}