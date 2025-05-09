using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Queries.GetRecipeById
{
    public class GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery, OperationResult<RecipeReadDto>>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetRecipeByIdQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<OperationResult<RecipeReadDto>> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
        {
            // Try to fetch the recipe by its ID from the repository
            // The repository returns an OperationResult (success or failure)
            return await _recipeRepository.GetRecipeByIdAsync(request.RecipeId);
        }
    }
}
