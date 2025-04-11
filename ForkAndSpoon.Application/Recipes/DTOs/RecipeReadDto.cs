namespace ForkAndSpoon.Application.Recipes.DTOs
{
    public class RecipeReadDto
    {
        public int RecipeID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Steps { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new();
        public List<string> DietaryPreferences { get; set; } = new();
    }
}
