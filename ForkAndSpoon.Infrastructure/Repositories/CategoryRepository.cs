using AutoMapper;
using ForkAndSpoon.Application.Categorys.DTOs;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ForkAndSpoonDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(ForkAndSpoonDbContext context, IMapper autoMapper)
        {
            _context = context;
            _mapper = autoMapper;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id) 
        { 
            var categoryInDb = await _context.Categories.FindAsync(id);

            if (categoryInDb == null) return null;

            return _mapper.Map<CategoryDto>(categoryInDb);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto newCategory)
        {
            var category = new Category
            {
                Name = newCategory.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category); // Make sure AutoMapper is injected
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int categoryId, CategoryUpdateDto categoryToUpdate)
        {
            var categoryInDb = await _context.Categories.FirstOrDefaultAsync(category => category.CategoryID == categoryId);

            if (categoryInDb == null) return null;

            _mapper.Map(categoryToUpdate, categoryInDb);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(categoryInDb);
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var categoryToDelete = await _context.Categories.FindAsync(categoryId);

            if (categoryToDelete == null || categoryToDelete.Name.ToLower() == "uncategorized")
                return false;

            _context.Categories.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}