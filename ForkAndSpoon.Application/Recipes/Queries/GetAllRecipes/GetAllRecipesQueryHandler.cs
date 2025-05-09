using AutoMapper;
using ForkAndSpoon.Application.Helpers;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Application.Recipes.Queries.GetAllRecipes
{
    public class GetAllRecipesQueryHandler : IRequestHandler<GetAllRecipesQuery, OperationResult<List<RecipeReadDto>>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _autoMapper;

        public GetAllRecipesQueryHandler(IRecipeRepository recipeRepository, IIngredientRepository ingredientRepository, IMapper autoMapper)
        {
            _recipeRepository = recipeRepository;
            _ingredientRepository = ingredientRepository;
            _autoMapper = autoMapper;
        }

        public async Task<OperationResult<List<RecipeReadDto>>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
        {
            try 
            {
                // Get base query of all non-deleted recipes
                var query = await _recipeRepository.GetAllRecipesQueryableAsync();

                // Apply optional filters (category, dietary, ingredient)
                query = RecipeQueryHelper.ApplyCategoryFilter(query, request.Category);
                query = RecipeQueryHelper.ApplyDietaryPreferenceFilter(query, request.Dietary);
                query = await RecipeQueryHelper.ApplyIngredientFilterAsync(query, request.Ingredient, _ingredientRepository);

                // Apply sorting (e.g. title_asc or title_desc)
                query = RecipeQueryHelper.ApplySorting(query, request.SortOrder);

                // Apply pagination
                query = RecipeQueryHelper.ApplyPagination(query, request.Page, request.PageSize);

                // Execute the query
                var paginatedResult = await query.ToListAsync();

                // Convert the result to DTOs
                var dtoList = _autoMapper.Map<List<RecipeReadDto>>(paginatedResult);

                // Return the successful result
                return OperationResult<List<RecipeReadDto>>.Success(dtoList);
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return OperationResult<List<RecipeReadDto>>.Failure($"Error retrieving recipes: {ex.Message}");
            }
        }
    }
}
