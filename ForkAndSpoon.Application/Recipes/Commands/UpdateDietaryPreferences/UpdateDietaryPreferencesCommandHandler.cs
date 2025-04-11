using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.UpdateDietaryPreferences
{
    public class UpdateDietaryPreferencesCommandHandler : IRequestHandler<UpdateDietaryPreferencesCommand, RecipeReadDto?>
    {
        private readonly IRecipeRepository _recipeRepository;

        public UpdateDietaryPreferencesCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipeReadDto?> Handle(UpdateDietaryPreferencesCommand request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.UpdateDietaryPreferencesAsync(
                request.RecipeId,
                request.UserId,
                request.UpdateDto
            );
        }
    }
}
