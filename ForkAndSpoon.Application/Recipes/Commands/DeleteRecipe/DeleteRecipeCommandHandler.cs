using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.DeleteRecipe
{
    public class DeleteRecipeCommandHandler : IRequestHandler<DeleteRecipeCommand, OperationResult<bool>>
    {
        private readonly IRecipeRepository _recipeRepository;

        public DeleteRecipeCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<OperationResult<bool>> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Load the recipe entity by ID
                var recipe = await _recipeRepository.GetRecipeEntityByIdAsync(request.RecipeId);

                // Check if recipe exists or is already deleted
                if (recipe == null || recipe.IsDeleted)
                    return OperationResult<bool>.Failure("Recipe not found or already deleted.");

                // Ensure only the creator or admin can delete the recipe
                if (recipe.CreatedBy != request.UserId && request.Role != "Admin")
                    return OperationResult<bool>.Failure("You are not authorized to delete this recipe.");

                // Mark recipe as deleted (soft delete)
                recipe.IsDeleted = true;

                // Save changes to database
                await _recipeRepository.SaveChangesAsync();

                // Return success
                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                // Handle undexpected errors
                return OperationResult<bool>.Failure($"Error deleting recipe: {ex.Message}");
            }
        }
    }
}
