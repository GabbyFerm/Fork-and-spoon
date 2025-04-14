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

        public async Task<bool> AddFavoriteAsync(int userId, int recipeId)
        {
            // Check if the recipe exists and is not soft-deleted
            var recipeExists = await _context.Recipes.AnyAsync(recipe => recipe.RecipeID == recipeId && !recipe.IsDeleted);

            if (!recipeExists)
            {
               throw new ArgumentException("Recipe does not exist or has been deleted.");
            }

            // Check if favorite already exists
            var alreadyFavorited = await _context.FavoriteRecipes.AnyAsync(f => f.UserID == userId && f.RecipeID == recipeId);

            if (alreadyFavorited)
                return false;

            var favorite = new FavoriteRecipe
            {
                UserID = userId,
                RecipeID = recipeId
            };

            _context.FavoriteRecipes.Add(favorite);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<RecipeReadDto>> GetUserFavoritesAsync(int userId)
        {
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

            return _autoMapper.Map<List<RecipeReadDto>>(favoriteRecipes);
        }

        public async Task<bool> RemoveFavoriteAsync(int userId, int recipeId)
        {
            var favorite = await _context.FavoriteRecipes.FirstOrDefaultAsync(favorite => favorite.UserID == userId && favorite.RecipeID == recipeId);

            if (favorite == null) return false;

            _context.FavoriteRecipes.Remove(favorite);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}