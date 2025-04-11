using ForkAndSpoon.Application.Recipes.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Queries.GetDeletedRecipeById
{
    public class GetDeletedRecipeByIdQuery : IRequest<RecipeReadDto?>
    {
        public int RecipeId { get; }

        public GetDeletedRecipeByIdQuery(int recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
