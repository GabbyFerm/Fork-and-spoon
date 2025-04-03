using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkAndSpoon.Domain.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public required string Name { get; set; }

        // Navigation Property
        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
