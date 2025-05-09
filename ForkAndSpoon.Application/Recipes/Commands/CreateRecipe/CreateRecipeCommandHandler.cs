using AutoMapper;
using ForkAndSpoon.Application.Factories;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Interfaces.Services;
using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, OperationResult<RecipeReadDto>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly ICategoryService _categoryService;
        private readonly IIngredientService _ingredientService;
        private readonly IDietaryPreferenceService _dietaryPreferenceService;
        private readonly IRecipeLoaderService _recipeLoaderService;
        private readonly IMapper _autoMapper;

        public CreateRecipeCommandHandler(
        IRecipeRepository recipeRepository,
        ICategoryService categoryService,
        IIngredientService ingredientService,
        IDietaryPreferenceService dietaryPreferenceService,
        IRecipeLoaderService recipeLoaderService,
        IMapper autoMapper)
        {
            _recipeRepository = recipeRepository;
            _categoryService = categoryService;
            _ingredientService = ingredientService;
            _dietaryPreferenceService = dietaryPreferenceService;
            _recipeLoaderService = recipeLoaderService;
            _autoMapper = autoMapper;
        }

        public async Task<OperationResult<RecipeReadDto>> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Prepare input data
                var dto = request.RecipeToCreate;

                // Validate or fallback to default category ID
                dto.CategoryID = await _categoryService.GetValidatedOrFallbackCategoryIdAsync(dto.CategoryID);

                // Create the base recipe object
                var newRecipe = RecipeFactory.CreateBaseRecipe(dto, request.UserId);

                // Add ingredients and dietary preferences to the recipe
                newRecipe.RecipeIngredients = await _ingredientService.BuildIngredientLinksAsync(dto.Ingredients, newRecipe);
                newRecipe.RecipeDietaryPreferences = await _dietaryPreferenceService.BuildDietaryPreferenceLinksAsync(dto.DietaryPreferences, newRecipe);

                // Save recipe to database
                var savedRecipe = await _recipeRepository.CreateRecipeAsync(newRecipe);

                // Reload the recipe with its full related data
                var fullRecipe = await _recipeLoaderService.GetRecipeWithRelationsAsync(savedRecipe.RecipeID);

                // If reload failed, return error message
                if (fullRecipe == null)
                    return OperationResult<RecipeReadDto>.Failure("Recipe was saved but could not be reloaded.");

                // Map the full recipe entity to a DTO
                var recipeDto = _autoMapper.Map<RecipeReadDto>(fullRecipe);

                // Return success with the mapped DTO
                return OperationResult<RecipeReadDto>.Success(recipeDto);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<RecipeReadDto>.Failure($"Error creating recipe: {ex.Message}");
            }
        }
    }
}
