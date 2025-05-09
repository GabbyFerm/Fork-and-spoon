using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Favorites.Queries.GetUserFavorites
{
    public class GetUserFavoritesQueryHandler : IRequestHandler<GetUserFavoritesQuery, OperationResult<List<RecipeReadDto>>>
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public GetUserFavoritesQueryHandler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<OperationResult<List<RecipeReadDto>>> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
        {
            return await _favoriteRepository.GetUserFavoritesAsync(request.UserId);
        }
    }
}
