using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces.Services
{
    public interface IRecipeLoaderService
    {
        // Loads a recipe including all related entities: Category, Ingredients, and DietaryPreferences.
        // Used to separate complex includes from the main repository.
        Task<Recipe?> GetRecipeWithRelationsAsync(int recipeId);
    }
}
