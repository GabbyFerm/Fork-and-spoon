using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Queries.GetDeletedRecipes
{
    public class GetDeletedRecipesQuery : IRequest<OperationResult<List<RecipeReadDto>>>
    {
        // Admin-only fetch, no properties needed
    }
}
