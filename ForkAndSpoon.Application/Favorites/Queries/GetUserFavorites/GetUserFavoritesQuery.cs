using ForkAndSpoon.Application.Recipes.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Favorites.Queries.GetUserFavorites
{
    public class GetUserFavoritesQuery : IRequest<List<RecipeReadDto>>
    {
        public int UserId { get; }

        public GetUserFavoritesQuery(int userId)
        {
            UserId = userId;
        }   

    }
}
