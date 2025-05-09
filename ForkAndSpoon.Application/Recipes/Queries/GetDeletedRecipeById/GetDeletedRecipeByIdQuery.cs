using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Queries.GetDeletedRecipeById
{
    public class GetDeletedRecipeByIdQuery : IRequest<OperationResult<RecipeReadDto>>
    {
        public int RecipeId { get; }

        public GetDeletedRecipeByIdQuery(int recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
