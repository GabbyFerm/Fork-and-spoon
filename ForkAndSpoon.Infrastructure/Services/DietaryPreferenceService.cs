using ForkAndSpoon.Application;
using ForkAndSpoon.Application.Interfaces.Services;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Infrastructure.Services
{
    public class DietaryPreferenceService : IDietaryPreferenceService
    {
        private readonly ForkAndSpoonDbContext _context;

        public DietaryPreferenceService(ForkAndSpoonDbContext context)
        {
            _context = context;
        }

        public async Task<List<RecipeDietaryPreference>> BuildDietaryPreferenceLinksAsync(List<string> preferenceNames, Recipe recipe)
        {
            var links = new List<RecipeDietaryPreference>();

            foreach (var rawName in preferenceNames)
            {
                // Clean and format the preference name
                var cleanedName = NameFormatter.Normalize(rawName);

                if (string.IsNullOrWhiteSpace(cleanedName))
                    continue;

                // Try to find an existing preference in the DB
                var existing = await _context.DietaryPreferences
                    .FirstOrDefaultAsync(p => p.Name.ToLower() == cleanedName.ToLower());

                // If it doesn't exist, create and add it to context
                if (existing == null)
                {
                    existing = new DietaryPreference { Name = cleanedName };
                    _context.DietaryPreferences.Add(existing);
                }

                // Create the join entity (many-to-many link)
                links.Add(new RecipeDietaryPreference
                {
                    Recipe = recipe,
                    DietaryPreference = existing
                });
            }

            return links;
        }
    }
}
