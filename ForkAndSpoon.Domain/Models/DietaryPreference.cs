namespace ForkAndSpoon.Domain.Models
{
    public class DietaryPreference
    {
        public int DietaryPreferenceID { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<RecipeDietaryPreference> RecipeDietaryPreferences { get; set; } = new List<RecipeDietaryPreference>();
    }
}
