using ForkAndSpoon.Application.Recipes.DTOs;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<List<RecipeReadDto>> GetUserFavoritesAsync(int userId);
        Task<bool> AddFavoriteAsync(int userId, int recipeId);
        Task<bool> RemoveFavoriteAsync(int userId, int recipeId);
    }
}
