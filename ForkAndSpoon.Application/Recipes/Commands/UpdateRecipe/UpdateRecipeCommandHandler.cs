using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, RecipeReadDto?>
    {
        private readonly IRecipeRepository _recipeRepository;

        public UpdateRecipeCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipeReadDto?> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.UpdateRecipeAsync(
                request.RecipeId,
                request.UpdatedRecipe,
                request.UserId,
                request.Role
            );
        }
    }

}
