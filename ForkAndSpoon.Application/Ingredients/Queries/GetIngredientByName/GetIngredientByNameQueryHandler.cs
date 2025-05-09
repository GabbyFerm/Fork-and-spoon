using AutoMapper;
using ForkAndSpoon.Application.Ingredients.DTOs;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Ingredients.Queries.GetIngredientByName
{
    public class GetIngredientByNameQueryHandler : IRequestHandler<GetIngredientByNameQuery, OperationResult<IngredientReadDto>>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _autoMapper;

        public GetIngredientByNameQueryHandler(IIngredientRepository ingredientRepository, IMapper autoMapper)
        {
            _ingredientRepository = ingredientRepository;
            _autoMapper = autoMapper;
        }

        public async Task<OperationResult<IngredientReadDto>> Handle(GetIngredientByNameQuery request, CancellationToken cancellationToken)
        {
            // Try to find the ingredient by name in the database
            var result = await _ingredientRepository.GetByNameAsync(request.Name);

            // If not found or failed, return an error
            if (!result.IsSuccess)
                return OperationResult<IngredientReadDto>.Failure(result.ErrorMessage ?? "Ingredient not found.");

            // Map the found ingredient entity to a DTO
            var dto = _autoMapper.Map<IngredientReadDto>(result.Data!);

            // Return success with the mapped DTO
            return OperationResult<IngredientReadDto>.Success(dto);
        }
    }
}
