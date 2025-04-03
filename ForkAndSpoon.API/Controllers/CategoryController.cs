using Microsoft.AspNetCore.Mvc;
using ForkAndSpoon.Application.DTOs.Category;
using ForkAndSpoon.Application.Interfaces;

namespace ForkAndSpoon.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("list-all-categories")]
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("get-category/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) 
                return NotFound("Category not found.");
            return Ok(category);
        }

        [HttpPost("create-category")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryToCreate)
        {
            var createdCategory = await _categoryService.CreateCategoryAsync(categoryToCreate);

            // Returns status code 201 created and location header
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.CategoryID }, createdCategory); 
        }

        [HttpPut("update-category/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto categoryToUpdate)
        {
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryToUpdate);

            if (updatedCategory == null) 
                return NotFound("Category not found.");

            return Ok(updatedCategory);
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var isDeleted = await _categoryService.DeleteCategoryAsync(id);

            if (!isDeleted) 
                return NotFound("Category not found or could not be deleted.");

            return NoContent();
        }
    }
}