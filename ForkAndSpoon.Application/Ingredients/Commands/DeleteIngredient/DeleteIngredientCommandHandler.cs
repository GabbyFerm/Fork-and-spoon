using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Ingredients.Commands.DeleteIngredient
{
    public class DeleteIngredientCommandHandler : IRequestHandler<DeleteIngredientCommand, OperationResult<bool>>
    {
        private readonly IIngredientRepository _ingredientRepository;

        public DeleteIngredientCommandHandler(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<OperationResult<bool>> Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
        {
            // Try to find the ingredient in the database by its ID
            var existingResult = await _ingredientRepository.GetByIdAsync(request.IngredientId);

            // If the ingredient doesn't exist, return a failure result
            if (!existingResult.IsSuccess || existingResult.Data == null)
                return OperationResult<bool>.Failure("Ingredient not found.");

            // Delete the ingredient and return the result
            return await _ingredientRepository.DeleteAsync(existingResult.Data);
        }
    }
}
