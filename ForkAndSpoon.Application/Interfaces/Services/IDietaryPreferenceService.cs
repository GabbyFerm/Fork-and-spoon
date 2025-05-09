using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces.Services
{
    public interface IDietaryPreferenceService
    {
        // Creates links between a recipe and dietary preferences based on the provided names.
        Task<List<RecipeDietaryPreference>> BuildDietaryPreferenceLinksAsync(List<string> preferenceNames, Recipe recipe);
    }
}
