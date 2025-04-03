using AutoMapper;
using ForkAndSpoon.Application.DTOs.Category;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ForkAndSpoonDbContext _context;
        private readonly IMapper _autoMapper;

        public CategoryService(ForkAndSpoonDbContext databaseContext, IMapper autoMapper)
        {
            _context = databaseContext;
            _autoMapper = autoMapper;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return _autoMapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id) 
        { 
            var categoryInDb = await _context.Categories.FindAsync(id);

            if (categoryInDb == null) return null;

            return _autoMapper.Map<CategoryDto>(categoryInDb);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto categoryToCreate)
        {
            var newCategory = _autoMapper.Map<Category>(categoryToCreate);

            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            return _autoMapper.Map<CategoryDto>(newCategory);
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int categoryId, CategoryUpdateDto categoryToUpdate)
        {
            var categoryInDb = await _context.Categories.FirstOrDefaultAsync(category => category.CategoryID == categoryId);

            if (categoryInDb == null) return null;

            _autoMapper.Map(categoryToUpdate, categoryInDb);
            await _context.SaveChangesAsync();

            return _autoMapper.Map<CategoryDto>(categoryInDb);
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var categoryToDelete = await _context.Categories.FindAsync(categoryId);
            
            if (categoryToDelete == null) return false;

            _context.Categories.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}