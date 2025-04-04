namespace ForkAndSpoon.Domain.Models
{
    public class Rating
    {
        public int RatingID { get; set; }
        public int? UserID { get; set; }
        public int? RecipeID { get; set; }

        public int Score { get; set; } // 1-5 scale

        // Navigation properties
        public User? User { get; set; }
        public Recipe? Recipe { get; set; }
    }
}
