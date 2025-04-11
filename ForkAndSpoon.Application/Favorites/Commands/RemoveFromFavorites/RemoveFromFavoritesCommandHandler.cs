using ForkAndSpoon.Application.Interfaces;
using MediatR;

namespace ForkAndSpoon.Application.Favorites.Commands.RemoveFromFavorites
{
    public class RemoveFromFavoritesCommandHandler : IRequestHandler<RemoveFromFavoritesCommand, bool>
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public RemoveFromFavoritesCommandHandler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<bool> Handle(RemoveFromFavoritesCommand request, CancellationToken cancellationToken)
        {
            return await _favoriteRepository.RemoveFavoriteAsync(request.UserId, request.RecipeId);
        }
    }
}
