using AutoMapper;
using ForkAndSpoon.Application.Ingredients.DTOs;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Ingredients.Queries.GetAllIngredients
{
    public class GetAllIngredientsQueryHandler : IRequestHandler<GetAllIngredientsQuery, OperationResult<List<IngredientReadDto>>>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _autoMapper;

        public GetAllIngredientsQueryHandler(IIngredientRepository ingredientRepository, IMapper autoMapper)
        {
            _ingredientRepository = ingredientRepository;
            _autoMapper = autoMapper;
        }

        public async Task<OperationResult<List<IngredientReadDto>>> Handle(GetAllIngredientsQuery request, CancellationToken cancellationToken)
        {
            // Fetch all ingredients from the database
            var result = await _ingredientRepository.GetAllAsync();

            // If the fetch failed, return an error
            if (!result.IsSuccess)
                return OperationResult<List<IngredientReadDto>>.Failure(result.ErrorMessage!);

            // Map the list of entities to DTOs
            var dtoList = _autoMapper.Map<List<IngredientReadDto>>(result);

            // Return the list of DTOs as a success result
            return OperationResult<List<IngredientReadDto>>.Success(dtoList);
        }
    }
}
