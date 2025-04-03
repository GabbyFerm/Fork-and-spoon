using ForkAndSpoon.Application.DTOs.Favorite;
using ForkAndSpoon.Application.DTOs.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface IFavoriteService
    {
        Task<List<RecipeReadDto>> GetUserFavoritesAsync(int userId);
        Task AddFavoriteAsync(int userId, int recipeId);
        Task<bool> RemoveFavoriteAsync(int userId, int recipeId);
    }
}
