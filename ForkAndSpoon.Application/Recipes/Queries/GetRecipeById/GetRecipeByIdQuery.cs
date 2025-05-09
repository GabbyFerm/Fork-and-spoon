using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Queries.GetRecipeById
{
    public class GetRecipeByIdQuery : IRequest<OperationResult<RecipeReadDto>>
    {
        public int RecipeId { get; }

        public GetRecipeByIdQuery(int recipeId)
        {
            RecipeId = recipeId; 
        }
    }
}
