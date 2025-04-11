using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Queries.GetAllRecipes
{
    public class GetAllRecipesQueryHandler : IRequestHandler<GetAllRecipesQuery, List<RecipeReadDto>>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetAllRecipesQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<List<RecipeReadDto>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.GetAllRecipesAsync(
                request.Category, request.Ingredient, request.Dietary, request.SortOrder, request.Page, request.PageSize
            );
        }
    }
}
