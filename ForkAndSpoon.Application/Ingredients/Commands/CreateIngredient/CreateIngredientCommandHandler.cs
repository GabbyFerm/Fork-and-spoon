using AutoMapper;
using ForkAndSpoon.Application.Ingredients.DTOs;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Ingredients.Commands.CreateIngredient
{
    public class CreateIngredientCommandHandler : IRequestHandler<CreateIngredientCommand, OperationResult<IngredientReadDto>>
    {
        private readonly IIngredientRepository _ingredientRepository;

        private readonly IMapper _autoMapper;

        public CreateIngredientCommandHandler(IIngredientRepository ingredientRepository, IMapper autoMapper)
        {
            _ingredientRepository = ingredientRepository;
            _autoMapper = autoMapper;
        }

        public async Task<OperationResult<IngredientReadDto>> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
        {
            // Check if an ingredient with the same name already exists in the database
            var existingIngredient = await _ingredientRepository.GetByNameAsync(request.Name);
            if (existingIngredient.IsSuccess && existingIngredient.Data != null)
                return OperationResult<IngredientReadDto>.Failure("Ingredient already exists.");

            // Create a new Ingredient entity from the name in the request
            var newIngredient = new Ingredient { Name = request.Name };

            // Try to save the new ingredient to the database
            var result = await _ingredientRepository.CreateAsync(newIngredient);

            // If something went wrong, return an error message
            if (!result.IsSuccess || result.Data == null)
                return OperationResult<IngredientReadDto>.Failure(result.ErrorMessage!);

            // Convert the Ingredient entity to a DTO for returning to the client
            var dto = _autoMapper.Map<IngredientReadDto>(result.Data);

            // Return the result wrapped in a success response
            return OperationResult<IngredientReadDto>.Success(dto);
        }
    }
}
