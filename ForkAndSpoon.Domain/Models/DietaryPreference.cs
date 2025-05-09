namespace ForkAndSpoon.Domain.Models
{
    public class DietaryPreference
    {
        public int DietaryPreferenceID { get; set; }
        public string Name { get; set; } = null!;

        // Recipes with this dietary preference (many-to-many)
        public ICollection<RecipeDietaryPreference> RecipeDietaryPreferences { get; set; } = new List<RecipeDietaryPreference>();
    }
}
