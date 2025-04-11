using ForkAndSpoon.Application.Recipes.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Queries.GetRecipeById
{
    public class GetRecipeByIdQuery : IRequest<RecipeReadDto?>
    {
        public int RecipeId { get; }

        public GetRecipeByIdQuery(int recipeId)
        {
            RecipeId = recipeId; 
        }
    }
}
