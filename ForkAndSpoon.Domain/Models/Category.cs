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
