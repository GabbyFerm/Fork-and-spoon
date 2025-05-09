using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Favorites.Queries.GetUserFavorites
{
    public class GetUserFavoritesQuery : IRequest<OperationResult<List<RecipeReadDto>>>
    {
        public int UserId { get; }

        public GetUserFavoritesQuery(int userId)
        {
            UserId = userId;
        }   

    }
}
