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
