using ForkAndSpoon.Application.Ingredients.DTOs;

namespace ForkAndSpoon.Application.Recipes.DTOs
{
    public class RecipeCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Steps { get; set; } = string.Empty;
        public int? CategoryID { get; set; }  // Select from categorylist or default to Uncategorized if not set
        public string? ImageUrl { get; set; }

        // Add dietatry preferences
        public List<string> DietaryPreferences { get; set; } = new();

        // Add ingredient and quantity
        public List<IngredientInputDto> Ingredients { get; set; } = new();
    }
}
