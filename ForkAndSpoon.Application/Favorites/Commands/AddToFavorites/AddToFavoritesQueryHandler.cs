using ForkAndSpoon.Application.Interfaces;
using MediatR;

namespace ForkAndSpoon.Application.Favorites.Commands.AddToFavorites
{
    public class AddToFavoritesQueryHandler : IRequestHandler<AddToFavoritesQuery, bool>
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public AddToFavoritesQueryHandler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<bool> Handle(AddToFavoritesQuery request, CancellationToken cancellationToken)
        {
            return await _favoriteRepository.AddFavoriteAsync(request.UserId, request.RecipeId);
        }
    }
}