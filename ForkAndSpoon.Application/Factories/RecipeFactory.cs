using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Factories
{
    public static class RecipeFactory
    {
        public static Recipe CreateBaseRecipe(RecipeCreateDto recipeCreateDto, int userId)
        {
            // Creates a new Recipe entity using basic fields from the RecipeCreateDto.
            // Related data like Ingredients and DietaryPreferences are added separately.

            var recipe = new Recipe
            {
                Title = recipeCreateDto.Title,
                Steps = recipeCreateDto.Steps,
                CategoryID = recipeCreateDto.CategoryID,
                ImageUrl = recipeCreateDto.ImageUrl,
                CreatedBy = userId
            };

            return recipe;
        }
    }
}
