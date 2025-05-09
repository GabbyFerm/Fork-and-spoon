using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Favorites.Commands.RemoveFromFavorites
{
    public class RemoveFromFavoritesCommand : IRequest<OperationResult<string>>
    {
        public int UserId { get; }
        public int RecipeId { get; }

        public RemoveFromFavoritesCommand(int userId, int recipeId)
        {
            UserId = userId;
            RecipeId = recipeId;
        }
    }
}