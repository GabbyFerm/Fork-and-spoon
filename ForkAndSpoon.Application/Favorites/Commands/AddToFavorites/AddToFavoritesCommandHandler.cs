using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Favorites.Commands.AddToFavorites
{
    public class AddToFavoritesCommandHandler : IRequestHandler<AddToFavoritesCommand, OperationResult<string>>
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public AddToFavoritesCommandHandler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<OperationResult<string>> Handle(AddToFavoritesCommand request, CancellationToken cancellationToken)
        {
            return await _favoriteRepository.AddFavoriteAsync(request.UserId, request.RecipeId);
        }
    }
}