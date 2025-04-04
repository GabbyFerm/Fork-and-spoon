using ForkAndSpoon.Application.DTOs.Recipe;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface IFavoriteService
    {
        Task<List<RecipeReadDto>> GetUserFavoritesAsync(int userId);
        Task AddFavoriteAsync(int userId, int recipeId);
        Task<bool> RemoveFavoriteAsync(int userId, int recipeId);
    }
}
