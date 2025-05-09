using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure.Database;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using ForkAndSpoon.Application;

namespace ForkAndSpoon.Infrastructure.Repositories
{
    public class IngredientRepository : GenericRepository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(ForkAndSpoonDbContext context) : base(context) { }

        public async Task<OperationResult<Ingredient>> GetByNameAsync(string name)
        {
            try
            {
                // Normalize the input name to ensure consistent lookups (e.g., "  SALT  " → "Salt")
                var cleanedName = NameFormatter.Normalize(name);

                // Look for the ingredient by its normalized name (case-insensitive)
                var ingredient = await _context.Ingredients
                    .FirstOrDefaultAsync(ingredient => ingredient.Name.ToLower() == cleanedName.ToLower());

                // If not found, return a failure message
                if (ingredient == null)
                    return OperationResult<Ingredient>.Failure("Ingredient not found.");

                // If found, return the ingredient wrapped in a success result
                return OperationResult<Ingredient>.Success(ingredient);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<Ingredient>.Failure($"Failed to get ingredient: {ex.Message}");
            }
        }
    }
}
