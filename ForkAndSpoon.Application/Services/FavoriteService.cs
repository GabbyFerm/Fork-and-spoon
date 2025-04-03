using AutoMapper;
using ForkAndSpoon.Application.DTOs.Recipe;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Application.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly ForkAndSpoonDbContext _context;
        private readonly IMapper _autoMapper;

        public FavoriteService(ForkAndSpoonDbContext context, IMapper autoMapper)
        {
            _context = context;
            _autoMapper = autoMapper;
        }

        public async Task AddFavoriteAsync(int userId, int recipeId)
        {
            var favoritesExist = await _context.Favorites.AnyAsync(favorite => favorite.UserID == userId && favorite.RecipeID == recipeId);

            if (favoritesExist) return;

            var favorite = new Favorite 
            { 
                UserID = userId,
                RecipeID = recipeId 
            };

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RecipeReadDto>> GetUserFavoritesAsync(int userId)
        {
            var favoriteRecipes = await _context.Favorites
                .Where(favorite => favorite.UserID == userId)
                .Select(favorite => favorite.Recipe)
                .Include(recipe => recipe.Category)
                .Include(recipe => recipe.RecipeIngredients)
                    .ThenInclude(ingredient => ingredient.Ingredient)
                .Include(recipe => recipe.RecipeDietaryPreferences)
                    .ThenInclude(dp => dp.DietaryPreference)
                .ToListAsync();

            return _autoMapper.Map<List<RecipeReadDto>>(favoriteRecipes);
        }

        public async Task<bool> RemoveFavoriteAsync(int userId, int recipeId)
        {
            var favorite = await _context.Favorites.FirstOrDefaultAsync(favorite => favorite.UserID == userId && favorite.RecipeID == recipeId);

            if (favorite == null) return false;

            _context.Favorites.Remove(favorite);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}