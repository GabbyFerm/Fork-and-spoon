namespace ForkAndSpoon.Domain.Models
{
    public class Ingredient
    {
        public int IngredientID { get; set; }
        public required string Name { get; set; }

        // Recipes that use this ingredient (many-to-many via RecipeIngredient)
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}
