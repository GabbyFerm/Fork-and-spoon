using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommand : IRequest<OperationResult<RecipeReadDto>>
    {
        public int RecipeId { get; }
        public RecipeUpdateDto UpdatedRecipe { get; }
        public int UserId { get; }
        public string UserRole { get; }

        public UpdateRecipeCommand(int recipeId, RecipeUpdateDto updatedRecipe, int userId, string userRole)
        {
            RecipeId = recipeId;
            UpdatedRecipe = updatedRecipe;
            UserId = userId;
            UserRole = userRole;
        }
    }
}
