using AutoMapper;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Queries.GetDeletedRecipeById
{
    public class GetDeletedRecipeByIdQueryHandler : IRequestHandler<GetDeletedRecipeByIdQuery, OperationResult<RecipeReadDto>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _autoMapper;

        public GetDeletedRecipeByIdQueryHandler(IRecipeRepository recipeRepository, IMapper autoMapper)
        {
            _recipeRepository = recipeRepository;
            _autoMapper = autoMapper;
        }

        public async Task<OperationResult<RecipeReadDto>> Handle(GetDeletedRecipeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Try to find the deleted recipe by ID
                var recipe = await _recipeRepository.GetDeletedRecipeByIdAsync(request.RecipeId);

                // If not found, return a failure message
                if (recipe == null)
                    return OperationResult<RecipeReadDto>.Failure("Deleted recipe not found.");

                // Map the recipe entity to a DTO
                var dto = _autoMapper.Map<RecipeReadDto>(recipe);

                // Return the result as a success with the mapped DTO
                return OperationResult<RecipeReadDto>.Success(dto);
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return OperationResult<RecipeReadDto>.Failure($"Error fetching deleted recipe: {ex.Message}");
            }
        }
    }
}
