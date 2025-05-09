namespace ForkAndSpoon.Domain.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public required string Name { get; set; }

        // Recipes in this category (1-to-many)
        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
