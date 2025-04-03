using ForkAndSpoon.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto newCategory);
        Task<CategoryDto?> UpdateCategoryAsync(int categoryId, CategoryUpdateDto categoryToUpdate);
        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}
