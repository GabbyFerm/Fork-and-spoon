using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.DeleteRecipe
{
    public class DeleteRecipeCommand : IRequest<bool>
    {
        public int RecipeId { get; }
        public int UserId { get; }
        public string Role { get; }

        public DeleteRecipeCommand(int recipeId, int userId, string role)
        {
            RecipeId = recipeId;
            UserId = userId;
            Role = role;
        }
    }

}
