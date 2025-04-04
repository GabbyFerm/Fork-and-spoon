using ForkAndSpoon.Application.DTOs.Recipe;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface IRecipeService
    {
        Task<List<RecipeReadDto>> GetAllRecipesAsync(string? categoryFilter = null, string? ingredientFilter = null, string? dietaryFilter = null, string ? sortOrder = null, int page = 1, int pageSize = 10);
        Task<RecipeReadDto?> GetRecipeByIdAsync(int id);
        Task<RecipeReadDto> CreateRecipeAsync(RecipeCreateDto dto, int userId);
        Task<RecipeReadDto?> UpdateRecipeAsync(int recipeId, RecipeUpdateDto updatedRecipe, int userId);
        Task<RecipeReadDto?> UpdateDietaryPreferencesAsync(int recipeId, int userId, RecipeDietaryPreferenceUpdateDto updateDto);
        Task<bool> DeleteRecipeAsync(int id);
        Task<List<RecipeReadDto>> GetDeletedRecipesAsync();
        Task<RecipeReadDto?> GetDeletedRecipeByIdAsync(int recipeId);
        Task<bool> RestoreDeletedRecipeAsync(int recipeId);
    }
}