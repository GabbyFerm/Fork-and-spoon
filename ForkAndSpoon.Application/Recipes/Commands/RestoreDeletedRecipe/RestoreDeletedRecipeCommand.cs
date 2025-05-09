using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.RestoreDeletedRecipe
{
    public class RestoreDeletedRecipeCommand : IRequest<OperationResult<bool>>
    {
        public int RecipeId { get; }

        public RestoreDeletedRecipeCommand(int recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
