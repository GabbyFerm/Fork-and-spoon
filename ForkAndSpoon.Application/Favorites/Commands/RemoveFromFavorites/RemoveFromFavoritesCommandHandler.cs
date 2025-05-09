using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Favorites.Commands.RemoveFromFavorites
{
    public class RemoveFromFavoritesCommandHandler : IRequestHandler<RemoveFromFavoritesCommand, OperationResult<string>>
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public RemoveFromFavoritesCommandHandler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<OperationResult<string>> Handle(RemoveFromFavoritesCommand request, CancellationToken cancellationToken)
        {
            return await _favoriteRepository.RemoveFavoriteAsync(request.UserId, request.RecipeId);
        }
    }
}
