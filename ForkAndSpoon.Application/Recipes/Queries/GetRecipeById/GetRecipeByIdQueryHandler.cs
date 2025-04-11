using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Queries.GetRecipeById
{
    public class GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery, RecipeReadDto?>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetRecipeByIdQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipeReadDto?> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.GetRecipeByIdAsync(request.RecipeId);
        }
    }
}
