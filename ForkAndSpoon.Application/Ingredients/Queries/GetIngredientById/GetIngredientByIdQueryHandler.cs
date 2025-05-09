using AutoMapper;
using ForkAndSpoon.Application.Ingredients.DTOs;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Ingredients.Queries.GetIngredientById
{
    public class GetIngredientByIdQueryHandler : IRequestHandler<GetIngredientByIdQuery, OperationResult<IngredientReadDto>>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _autoMapper;

        public GetIngredientByIdQueryHandler(IIngredientRepository ingredientRepository, IMapper autoMapper)
        {
            _ingredientRepository = ingredientRepository;
            _autoMapper = autoMapper;
        }

        public async Task<OperationResult<IngredientReadDto>> Handle(GetIngredientByIdQuery request, CancellationToken cancellationToken)
        {
            // Try to find the ingredient by ID in the database
            var result = await _ingredientRepository.GetByIdAsync(request.IngredientId);

            // If it failed, return an error
            if (!result.IsSuccess)
                return OperationResult<IngredientReadDto>.Failure(result.ErrorMessage!);

            // Map the entity to a DTO
            var dto = _autoMapper.Map<IngredientReadDto>(result.Data!);

            // Return the DTO wrapped in a success result
            return OperationResult<IngredientReadDto>.Success(dto);
        }
    }
}
