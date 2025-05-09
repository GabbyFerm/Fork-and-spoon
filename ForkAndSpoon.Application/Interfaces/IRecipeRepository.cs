using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces
{
    // Repository handles basic CRUD operations for Recipe entities.
    // For loading full relational data (e.g., Ingredients, Category, DietaryPreferences), use IRecipeLoaderService.

    public interface IRecipeRepository 
    {
        Task<IQueryable<Recipe>> GetAllRecipesQueryableAsync(); // Queryable for filtering, sorting, pagination (non-deleted recipes)
        Task<OperationResult<RecipeReadDto>> GetRecipeByIdAsync(int recipeId); // Returns a single recipe as a DTO
        Task<Recipe?> GetRecipeEntityByIdAsync(int recipeId); // Returns a recipe entity (used for internal update/delete logic)
        Task<Recipe> CreateRecipeAsync(Recipe recipe); // Creates a new recipe (returns entity)
        Task<List<Recipe>> GetDeletedRecipesAsync(); // Returns all soft-deleted recipes
        Task<Recipe?> GetDeletedRecipeByIdAsync(int recipeId); // Returns a deleted recipe with full relations
        Task<Recipe?> GetRecipeOwnedByUserAsync(int recipeId, int userId); // Ensures recipe is owned by the given user
        Task<OperationResult<bool>> SaveChangesAsync(); // Saves changes to the database
    }
}