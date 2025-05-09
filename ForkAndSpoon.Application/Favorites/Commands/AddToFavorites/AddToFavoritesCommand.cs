using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Favorites.Commands.AddToFavorites
{
    public class AddToFavoritesCommand : IRequest<OperationResult<string>>
    {
        public int UserId { get; }
        public int RecipeId { get; }

        public AddToFavoritesCommand(int userId, int recipeId) 
        { 
            UserId = userId;
            RecipeId = recipeId;
        }
    }
}