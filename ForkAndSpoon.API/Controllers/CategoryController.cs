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

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery());

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryInputDto newCategory)
        {
            var result = await _mediator.Send(new CreateCategoryCommand(newCategory));

            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetCategoryById), new { id = result.Data!.CategoryID }, result);
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryInputDto updateDto)
        {
            var result = await _mediator.Send(new UpdateCategoryCommand(id, updateDto.Name));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(id));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}