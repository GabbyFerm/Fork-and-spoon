using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.RestoreDeletedRecipe
{
    public class RestoreDeletedRecipeCommand : IRequest<bool>
    {
        public int RecipeId { get; }

        public RestoreDeletedRecipeCommand(int recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
