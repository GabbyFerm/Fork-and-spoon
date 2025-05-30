using ForkAndSpoon.Application.Interfaces.Services;
using ForkAndSpoon.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ForkAndSpoonDbContext _context;

        public CategoryService(ForkAndSpoonDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetValidatedOrFallbackCategoryIdAsync(int? categoryId)
        {
            // Check if provided categoryId exists in the database
            if (categoryId != null)
            {
                bool exists = await _context.Categories.AnyAsync(category => category.CategoryID == categoryId);
                if (exists) return categoryId.Value;
            }

            // If not found, use fallback: CategoryID = 1 ("Uncategorized")
            const int fallbackCategoryId = 1;
            bool fallbackExists = await _context.Categories.AnyAsync(category => category.CategoryID == fallbackCategoryId);

            // If fallback doesn't exist, throw a clear error
            if (!fallbackExists)
                throw new ArgumentException("No valid category selected and fallback 'Uncategorized' category is missing.");

            return fallbackCategoryId;
        }
    }
}
