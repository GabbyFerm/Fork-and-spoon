using MediatR;

namespace ForkAndSpoon.Application.Favorites.Commands.AddToFavorites
{
    public class AddToFavoritesQuery : IRequest<bool>
    {
        public int UserId { get; }
        public int RecipeId { get; }

        public AddToFavoritesQuery(int userId, int recipeId) 
        { 
            UserId = userId;
            RecipeId = recipeId;
        }
    }
}