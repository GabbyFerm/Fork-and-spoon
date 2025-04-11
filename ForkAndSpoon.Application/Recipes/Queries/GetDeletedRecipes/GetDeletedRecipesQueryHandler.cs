using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Queries.GetDeletedRecipes
{
    public class GetDeletedRecipesQueryHandler : IRequestHandler<GetDeletedRecipesQuery, List<RecipeReadDto>>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetDeletedRecipesQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<List<RecipeReadDto>> Handle(GetDeletedRecipesQuery request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.GetDeletedRecipesAsync();
        }
    }
}
