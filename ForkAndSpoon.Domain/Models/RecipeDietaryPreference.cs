namespace ForkAndSpoon.Domain.Models
{
    public class RecipeDietaryPreference
    {
        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; } = null!;

        public int DietaryPreferenceID { get; set; }
        public DietaryPreference DietaryPreference { get; set; } = null!;
    }
}
