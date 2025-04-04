namespace ForkAndSpoon.Domain.Models
{
    public class Favorite
    {
        public int UserID { get; set; }
        public User User { get; set; } = null!;

        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; } = null!;
    }
}
