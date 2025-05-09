using AutoMapper;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Interfaces.Services;
using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, OperationResult<RecipeReadDto>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IRecipeLoaderService _recipeLoaderService;
        private readonly IDietaryPreferenceService _dietaryPreferenceService;
        private readonly IMapper _mapper;

        public UpdateRecipeCommandHandler(
            IRecipeRepository recipeRepository,
            IRecipeLoaderService recipeLoaderService,
            IDietaryPreferenceService dietaryPreferenceService,
            IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _recipeLoaderService = recipeLoaderService;
            _dietaryPreferenceService = dietaryPreferenceService;
            _mapper = mapper;
        }

        public async Task<OperationResult<RecipeReadDto>> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Load recipe with full relations
                var recipe = await _recipeLoaderService.GetRecipeWithRelationsAsync(request.RecipeId);

                if (recipe == null || (recipe.CreatedBy != request.UserId && request.UserRole != "Admin"))
                    return OperationResult<RecipeReadDto>.Failure("Recipe not found or access denied.");

                // Update basic fields
                recipe.Title = request.UpdatedRecipe.Title;
                recipe.Steps = request.UpdatedRecipe.Steps;
                recipe.ImageUrl = request.UpdatedRecipe.ImageUrl;
                recipe.CategoryID = request.UpdatedRecipe.CategoryID;

                // Update dietary preferences
                recipe.RecipeDietaryPreferences.Clear();
                var newPreferences = await _dietaryPreferenceService
                    .BuildDietaryPreferenceLinksAsync(request.UpdatedRecipe.DietaryPreferences, recipe);

                recipe.RecipeDietaryPreferences = newPreferences;

                // Save changes to database
                await _recipeRepository.SaveChangesAsync();

                // Reload updated recipe with relations
                var updated = await _recipeLoaderService.GetRecipeWithRelationsAsync(request.RecipeId);

                // If reload failed, return error message
                if (updated == null)
                    return OperationResult<RecipeReadDto>.Failure("Recipe updated but failed to reload.");

                // Map the updated recipe entity to a DTO
                var dto = _mapper.Map<RecipeReadDto>(updated);

                // Return success with the mapped DTO
                return OperationResult<RecipeReadDto>.Success(dto);

            }

            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<RecipeReadDto>.Failure($"Error updating recipe: {ex.Message}");
            }
        }
    }
}
