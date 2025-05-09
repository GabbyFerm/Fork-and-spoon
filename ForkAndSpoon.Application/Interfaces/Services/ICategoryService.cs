namespace ForkAndSpoon.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        // Returns the provided category ID if valid; otherwise, returns the fallback "Uncategorized" category ID.
        Task<int> GetValidatedOrFallbackCategoryIdAsync(int? categoryId);
    }
}
