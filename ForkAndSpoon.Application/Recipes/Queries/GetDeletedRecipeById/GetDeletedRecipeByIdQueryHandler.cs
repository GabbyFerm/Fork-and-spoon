using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Queries.GetDeletedRecipeById
{
    public class GetDeletedRecipeByIdQueryHandler : IRequestHandler<GetDeletedRecipeByIdQuery, RecipeReadDto?>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetDeletedRecipeByIdQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipeReadDto?> Handle(GetDeletedRecipeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.GetDeletedRecipeByIdAsync(request.RecipeId);
        }
    }
}
