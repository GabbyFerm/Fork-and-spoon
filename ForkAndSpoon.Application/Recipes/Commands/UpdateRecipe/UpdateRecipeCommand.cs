using ForkAndSpoon.Application.Recipes.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommand : IRequest<RecipeReadDto?>
    {
        public int RecipeId { get; }
        public RecipeUpdateDto UpdatedRecipe { get; }
        public int UserId { get; }
        public string Role { get; }

        public UpdateRecipeCommand(int recipeId, RecipeUpdateDto updatedRecipe, int userId, string role)
        {
            RecipeId = recipeId;
            UpdatedRecipe = updatedRecipe;
            UserId = userId;
            Role = role;
        }
    }
}
