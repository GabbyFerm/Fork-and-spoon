using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.RestoreDeletedRecipe
{
    public class RestoreDeletedRecipeCommandHandler : IRequestHandler<RestoreDeletedRecipeCommand, OperationResult<bool>>
    {
        private readonly IRecipeRepository _recipeRepository;

        public RestoreDeletedRecipeCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<OperationResult<bool>> Handle(RestoreDeletedRecipeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrive the soft-deleted recipe by ID
                var recipe = await _recipeRepository.GetRecipeEntityByIdAsync(request.RecipeId);

                // Check if the recipe exists and is actually deleted
                if (recipe == null || !recipe.IsDeleted)
                    return OperationResult<bool>.Failure("Deleted recipe not found.");

                // Restore the recipe by setting IsDeleted to false
                recipe.IsDeleted = false;

                // Save the changes to the database
                await _recipeRepository.SaveChangesAsync();

                // Return success result
                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<bool>.Failure($"Error restoring recipe: {ex.Message}");
            }
        }
    }
}
