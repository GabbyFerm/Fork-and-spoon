namespace ForkAndSpoon.Domain.Models
{
    public class Recipe
    {
        public int RecipeID { get; set; }
        public required string Title { get; set; }
        public required string Steps { get; set; }
        public int? CategoryID { get; set; }
        public string? ImageUrl { get; set; }

        public bool IsDeleted { get; set; } // Soft delete

        public int? CreatedBy { get; set; } // Foreign key
        public User? CreatedByUser { get; set; } // Navigation to creator

        // Navigation
        public Category? Category { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
        public ICollection<RecipeDietaryPreference> RecipeDietaryPreferences { get; set; } = new List<RecipeDietaryPreference>();
        public ICollection<FavoriteRecipe> FavoriteRecipes { get; set; } = new List<FavoriteRecipe>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}