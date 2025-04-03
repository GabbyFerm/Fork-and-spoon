using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkAndSpoon.Application.DTOs.Recipe
{
    public class RecipeUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Steps { get; set; } = string.Empty;
        public int CategoryID { get; set; }
        public string? ImageUrl { get; set; }
        public List<string> Ingredients { get; set; } = new();
        public List<string> DietaryPreferences { get; set; } = new();
    }
}
