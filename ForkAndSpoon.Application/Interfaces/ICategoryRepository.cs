using ForkAndSpoon.Application.Categorys.DTOs;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto newCategory);
        Task<CategoryDto?> UpdateCategoryAsync(int categoryId, CategoryUpdateDto categoryToUpdate);
        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}
