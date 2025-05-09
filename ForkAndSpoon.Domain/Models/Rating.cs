namespace ForkAndSpoon.Domain.Models
{
    public class Rating
    {
        public int RatingID { get; set; }
        public int? UserID { get; set; }
        public int? RecipeID { get; set; }

        public int Score { get; set; } // 1 to 5 stars

        // Navigation
        public User? User { get; set; }
        public Recipe? Recipe { get; set; }
    }
}
