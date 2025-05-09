using AutoMapper;
using ForkAndSpoon.Application.Ingredients.DTOs;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Ingredients.Commands.UpdateIngredient
{
    public class UpdateIngredientCommandHandler : IRequestHandler<UpdateIngredientCommand, OperationResult<IngredientReadDto>>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _autoMapper;

        public UpdateIngredientCommandHandler(IIngredientRepository ingredientRepository, IMapper autoMapper)
        {
            _ingredientRepository = ingredientRepository;
            _autoMapper = autoMapper;
        }

        public async Task<OperationResult<IngredientReadDto>> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
        {
            // Try to find the ingredient in the database by its ID
            var existingResult = await _ingredientRepository.GetByIdAsync(request.IngredientId);

            // If it doesn't exist, return a failure result
            if (!existingResult.IsSuccess || existingResult.Data == null)
                return OperationResult<IngredientReadDto>.Failure("Ingredient not found.");

            // Update the ingredient's name with the new value
            existingResult.Data.Name = request.UpdatedName;

            // Save the changes to the database
            var updateResult = await _ingredientRepository.UpdateAsync(existingResult.Data);

            // If the update failed, return a failure result
            if (!updateResult.IsSuccess || updateResult.Data == null)
                return OperationResult<IngredientReadDto>.Failure(updateResult.ErrorMessage ?? "Update failed.");

            // Convert the updated entity to a DTO and return it
            var dto = _autoMapper.Map<IngredientReadDto>(updateResult.Data);
            return OperationResult<IngredientReadDto>.Success(dto);
        }
    }
}
