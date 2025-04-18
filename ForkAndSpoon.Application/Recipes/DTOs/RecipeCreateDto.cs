﻿namespace ForkAndSpoon.Application.Recipes.DTOs
{
    public class RecipeCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Steps { get; set; } = string.Empty;
        public int? CategoryID { get; set; }  // Select from categorylist or default to Uncategorized if not set
        public string? ImageUrl { get; set; }

        // Ingredient names to be added with the recipe
        public List<string> Ingredients { get; set; } = new();

        // Add dietatry preferences
        public List<string> DietaryPreferences { get; set; } = new();
    }
}
