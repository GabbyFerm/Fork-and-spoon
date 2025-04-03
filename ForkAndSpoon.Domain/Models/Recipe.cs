using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkAndSpoon.Domain.Models
{
    public class Recipe
    {
        public int RecipeID { get; set; }
        public required string Title { get; set; }
        public required string Steps { get; set; }
        public int CategoryID { get; set; }
        public string? ImageUrl { get; set; } // Image is not required

        // Foreign key for user (creator)
        public int CreatedBy { get; set; }
        public User? CreatedByUser { get; set; }

        // Navigation properties
        public Category? Category { get; set; }

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
        public ICollection<RecipeDietaryPreference> RecipeDietaryPreferences { get; set; } = new List<RecipeDietaryPreference>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
