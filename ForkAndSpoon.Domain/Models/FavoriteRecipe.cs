namespace ForkAndSpoon.Domain.Models
{
    public class FavoriteRecipe
    {
        // User who favorited the recipe
        public int UserID { get; set; }
        public User User { get; set; } = null!;

        // Favorited recipe
        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; } = null!;
    }
}
