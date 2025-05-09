using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface IFavoriteRepository
    {
        // Gets all favorite recipes for a given user.
        Task<OperationResult<List<RecipeReadDto>>> GetUserFavoritesAsync(int userId);

        // Adds a recipe to the user's list of favorites.
        Task<OperationResult<string>> AddFavoriteAsync(int userId, int recipeId);

        // Removes a recipe from the user's list of favorites.
        Task<OperationResult<string>> RemoveFavoriteAsync(int userId, int recipeId);
    }
}
