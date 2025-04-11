using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Favorites.Queries.GetUserFavorites
{
    public class GetUserFavoritesQueryHandler : IRequestHandler<GetUserFavoritesQuery, List<RecipeReadDto>>
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public GetUserFavoritesQueryHandler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<List<RecipeReadDto>> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
        {
            return await _favoriteRepository.GetUserFavoritesAsync(request.UserId);
        }
    }
}
