using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkAndSpoon.Domain.Models
{
    public class Ingredient
    {
        public int IngredientID { get; set; }
        public required string Name { get; set; }

        // Many-to-Many with RecipeIngredient
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}
