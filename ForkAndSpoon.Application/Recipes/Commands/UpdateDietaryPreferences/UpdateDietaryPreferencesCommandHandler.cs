using AutoMapper;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Interfaces.Services;
using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.UpdateDietaryPreferences
{
    public class UpdateDietaryPreferencesCommandHandler : IRequestHandler<UpdateDietaryPreferencesCommand, OperationResult<RecipeReadDto>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IDietaryPreferenceService _dietaryPreferenceService;
        private readonly IRecipeLoaderService _recipeLoaderService;
        private readonly IMapper _autoMapper;

        public UpdateDietaryPreferencesCommandHandler(IRecipeRepository recipeRepository, IDietaryPreferenceService dietaryPreferenceService, IRecipeLoaderService recipeLoaderService, IMapper autoMapper)
        {
            _recipeRepository = recipeRepository;
            _dietaryPreferenceService = dietaryPreferenceService;
            _recipeLoaderService = recipeLoaderService;
            _autoMapper = autoMapper;
        }

        public async Task<OperationResult<RecipeReadDto>> Handle(UpdateDietaryPreferencesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Load recipe owned by the current user
                var recipe = await _recipeRepository.GetRecipeOwnedByUserAsync(request.RecipeId, request.UserId);

                if (recipe == null)
                    return OperationResult<RecipeReadDto>.Failure("Recipe not found or you do not have permission to update dietary preferences.");

                // Clear old preferences
                recipe.RecipeDietaryPreferences.Clear();

                // Build and assign new preferences
                var newPreferences = await _dietaryPreferenceService.BuildDietaryPreferenceLinksAsync(
                    request.UpdateDto.DietaryPreferences, recipe
                );
                recipe.RecipeDietaryPreferences = newPreferences;

                // Save changes to database
                await _recipeRepository.SaveChangesAsync();

                // Reload recipe with all relations
                var updatedRecipe = await _recipeLoaderService.GetRecipeWithRelationsAsync(recipe.RecipeID);

                // Return error message when fail to load recipe
                if (updatedRecipe == null)
                    return OperationResult<RecipeReadDto>.Failure("Updated preferences but failed to reload recipe.");

                // Map and return result
                var dto = _autoMapper.Map<RecipeReadDto>(updatedRecipe);

                // Return success
                return OperationResult<RecipeReadDto>.Success(dto);
            }
            catch (Exception ex)
            {
                // Handle undexpected errors
                return OperationResult<RecipeReadDto>.Failure($"Error updating dietary preferences: {ex.Message}");
            }
        }
    }
}