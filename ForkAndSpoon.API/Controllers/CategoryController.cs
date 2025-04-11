using ForkAndSpoon.API.Helpers;
using ForkAndSpoon.Application.Categorys.Commands.CreateCategory;
using ForkAndSpoon.Application.Categorys.Commands.DeleteCategory;
using ForkAndSpoon.Application.Categorys.Commands.UpdateCategory;
using ForkAndSpoon.Application.Categorys.DTOs;
using ForkAndSpoon.Application.Categorys.Queries.GetAllCategories;
using ForkAndSpoon.Application.Categorys.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForkAndSpoon.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("list-all")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());

            return Ok(categories);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(id));

            if (category == null) 
                return NotFound("Category not found.");

            return Ok(category);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
        {
            var command = new CreateCategoryCommand(categoryCreateDto.Name);
            var createdCategory = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.CategoryID }, createdCategory);
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] CategoryUpdateDto updateDto)
        {
            var role = ClaimsHelper.GetUserRoleFromClaims(User);

            var command = new UpdateCategoryCommand(id, updateDto.Name, role);

            try
            {
                var updatedCategory = await _mediator.Send(command);
                return Ok(updatedCategory);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid("Only admins can update this category."); 
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var role = ClaimsHelper.GetUserRoleFromClaims(User);

            var command = new DeleteCategoryCommand(id, role);
            var success = await _mediator.Send(command);

            if (!success)
                return NotFound("Category not found or could not be deleted.");

            return NoContent();
        }
    }
}