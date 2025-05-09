using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.UpdateDietaryPreferences
{
    public class UpdateDietaryPreferencesCommand : IRequest<OperationResult<RecipeReadDto>>
    {
        public int RecipeId { get; }
        public int UserId { get; }
        public RecipeDietaryPreferenceUpdateDto UpdateDto { get; }

        public UpdateDietaryPreferencesCommand(int recipeId, int userId, RecipeDietaryPreferenceUpdateDto updateDto)
        {
            RecipeId = recipeId;
            UserId = userId;
            UpdateDto = updateDto;
        }
    }
}
