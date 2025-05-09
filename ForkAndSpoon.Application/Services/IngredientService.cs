using ForkAndSpoon.Application.Ingredients.DTOs;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Interfaces.Services;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;

        public IngredientService(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<List<RecipeIngredient>> BuildIngredientLinksAsync(List<IngredientInputDto> ingredientInputs, Recipe recipe)
        {
            var recipeIngredients = new List<RecipeIngredient>();

            foreach (var input in ingredientInputs)
            {
                // Normalize and clean up the ingredient name (e.g. trim spaces, standardize case)
                var cleanedName = NameFormatter.Normalize(input.Name);

                // Skip empty or whitespace-only names
                if (string.IsNullOrWhiteSpace(cleanedName))
                    continue;

                // Check if the ingredient already exists in the database
                var result = await _ingredientRepository.GetByNameAsync(cleanedName);

                Ingredient ingredient;
                if (result.IsSuccess && result.Data != null)
                {
                    // Use the existing ingredient from the database
                    ingredient = result.Data;
                }
                else
                {
                    // Create a new ingredient if not found
                    ingredient = new Ingredient { Name = cleanedName };
                }

                // Create the RecipeIngredient link object
                var recipeIngredient = new RecipeIngredient
                {
                    Ingredient = ingredient,
                    Recipe = recipe,
                    Quantity = input.Quantity
                };

                // Add the link to the list
                recipeIngredients.Add(recipeIngredient);
            }

            // Return all ingredient links for the recipe
            return recipeIngredients;
        }
    }
}
