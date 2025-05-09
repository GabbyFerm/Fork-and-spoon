using ForkAndSpoon.Application.Ingredients.DTOs;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces.Services
{
    public interface IIngredientService
    {
        // Creates links between a recipe and ingredients, including quantities, based on input data.
        Task<List<RecipeIngredient>> BuildIngredientLinksAsync(List<IngredientInputDto> ingredientInputs, Recipe recipe);
    }
}
