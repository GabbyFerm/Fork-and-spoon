using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Factories
{
    public static class RecipeFactory
    {
        public static Recipe CreateRecipeFromDto(RecipeCreateDto recipeCreateDto, int userId)
        {
            var recipe = new Recipe
            {
                Title = recipeCreateDto.Title,
                Steps = recipeCreateDto.Steps,
                CategoryID = recipeCreateDto.CategoryID,
                ImageUrl = recipeCreateDto.ImageUrl,
                CreatedBy = userId
            };

            // Ingredients
            foreach (var ingredientName in recipeCreateDto.Ingredients)
            {
                var ingredient = new Ingredient { Name = ingredientName };

                var recipeIngredient = new RecipeIngredient
                {
                    Ingredient = ingredient,
                    Recipe = recipe
                };

                recipe.RecipeIngredients.Add(recipeIngredient);
            }

            return recipe;
        }
    }
}
