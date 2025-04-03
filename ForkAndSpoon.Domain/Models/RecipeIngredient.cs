using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkAndSpoon.Domain.Models
{
    public class RecipeIngredient
    {
        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; } = null!;

        public int IngredientID { get; set; }
        public Ingredient Ingredient { get; set; } = null!;
    }
}
