namespace ForkAndSpoon.Domain.Models
{
    public class User
    {
        public int UserID { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        public string Role { get; set; } = "User"; // Default role to those who registers

        // Navigation properties
        public ICollection<Recipe> CreatedRecipes { get; set; } = new List<Recipe>();
        public ICollection<FavoriteRecipe> FavoriteRecipes { get; set; } = new List<FavoriteRecipe>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
