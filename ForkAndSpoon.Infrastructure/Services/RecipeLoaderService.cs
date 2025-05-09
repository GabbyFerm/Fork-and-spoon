using ForkAndSpoon.Application.Interfaces.Services;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Infrastructure.Services
{
    public class RecipeLoaderService : IRecipeLoaderService
    {
        private readonly ForkAndSpoonDbContext _context;

        public RecipeLoaderService(ForkAndSpoonDbContext context)
        {
            _context = context;
        }

        // This service loads a full Recipe entity with related data:
        // Category, Ingredients, Dietary Preferences

        public async Task<Recipe?> GetRecipeWithRelationsAsync(int recipeId)
        {
            return await _context.Recipes
                .Include(recipe => recipe.Category)
                .Include(recipe => recipe.RecipeIngredients)
                    .ThenInclude(recipeIngredient => recipeIngredient.Ingredient)
                .Include(recipe => recipe.RecipeDietaryPreferences)
                    .ThenInclude(dietaryP => dietaryP.DietaryPreference)
                .FirstOrDefaultAsync(recipe => recipe.RecipeID == recipeId);
        }
    }
}
