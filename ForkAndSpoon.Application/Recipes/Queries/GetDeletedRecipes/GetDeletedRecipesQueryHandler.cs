using AutoMapper;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Queries.GetDeletedRecipes
{
    public class GetDeletedRecipesQueryHandler : IRequestHandler<GetDeletedRecipesQuery, OperationResult<List<RecipeReadDto>>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _autoMapper;

        public GetDeletedRecipesQueryHandler(IRecipeRepository recipeRepository, IMapper autoMapper)
        {
            _recipeRepository = recipeRepository;
            _autoMapper = autoMapper;
        }

        public async Task<OperationResult<List<RecipeReadDto>>> Handle(GetDeletedRecipesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch all recipes that have been marked as deleted
                var deletedRecipes = await _recipeRepository.GetDeletedRecipesAsync();

                // Map the list of recipe entities to a list of DTOs
                var dtoList = _autoMapper.Map<List<RecipeReadDto>>(deletedRecipes);

                // Return the list of deleted recipes as a successful result
                return OperationResult<List<RecipeReadDto>>.Success(dtoList);
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return OperationResult<List<RecipeReadDto>>.Failure($"Error fetching deleted recipes: {ex.Message}");
            }
        }
    }
}
