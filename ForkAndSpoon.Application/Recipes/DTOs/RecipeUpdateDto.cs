using ForkAndSpoon.Application.Ingredients.DTOs;

namespace ForkAndSpoon.Application.Recipes.DTOs
{
    public class RecipeUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Steps { get; set; } = string.Empty;
        public int CategoryID { get; set; }
        public string? ImageUrl { get; set; }
        public List<IngredientInputDto> Ingredients { get; set; } = new();
        public List<string> DietaryPreferences { get; set; } = new();
    }
}
