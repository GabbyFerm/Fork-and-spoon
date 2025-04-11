using ForkAndSpoon.Application.Recipes.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Queries.GetDeletedRecipes
{
    public class GetDeletedRecipesQuery : IRequest<List<RecipeReadDto>>
    {
        // Admin-only fetch, no properties needed
    }
}
