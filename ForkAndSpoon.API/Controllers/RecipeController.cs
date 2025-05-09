using ForkAndSpoon.API.Helpers;
using ForkAndSpoon.Application.Categorys.Commands.CreateCategory;
using ForkAndSpoon.Application.Categorys.DTOs;
using ForkAndSpoon.Application.Recipes.Commands.CreateRecipe;
using ForkAndSpoon.Application.Recipes.Commands.DeleteRecipe;
using ForkAndSpoon.Application.Recipes.Commands.RestoreDeletedRecipe;
using ForkAndSpoon.Application.Recipes.Commands.UpdateDietaryPreferences;
using ForkAndSpoon.Application.Recipes.Commands.UpdateRecipe;
using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Application.Recipes.Queries.GetAllRecipes;
using ForkAndSpoon.Application.Recipes.Queries.GetDeletedRecipeById;
using ForkAndSpoon.Application.Recipes.Queries.GetDeletedRecipes;
using ForkAndSpoon.Application.Recipes.Queries.GetRecipeById;
using ForkAndSpoon.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RecipeController : ControllerBase
{
    private readonly IMediator _mediator;

    public RecipeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-recipes")]
    public async Task<IActionResult> GetAllRecipes(
    [FromQuery] string? category,
    [FromQuery] string? ingredient,
    [FromQuery] string? dietary,
    [FromQuery] string? sortOrder,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllRecipesQuery(category, ingredient, dietary, sortOrder, page, pageSize));

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return Ok(result);
    }

    [HttpGet("get-recipe/{id}")]
    public async Task<IActionResult> GetRecipeById(int id)
    {
        var result = await _mediator.Send(new GetRecipeByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("create-recipe")]
    public async Task<IActionResult> CreateRecipe([FromBody] RecipeCreateDto recipeToCreate)
    {
        var userId = ClaimsHelper.GetUserIdFromClaims(User);

        var command = new CreateRecipeCommand(recipeToCreate, userId);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return CreatedAtAction(nameof(GetRecipeById), new { id = result.Data!.RecipeID }, result);
    }

    [Authorize]
    [HttpPut("update-recipe/{id}")]
    public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeUpdateDto updatedRecipe)
    {
        var userId = ClaimsHelper.GetUserIdFromClaims(User);
        var userRole = ClaimsHelper.GetUserRoleFromClaims(User);

        var command = new UpdateRecipeCommand(id, updatedRecipe, userId, userRole);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return Ok(result);
    }

    [Authorize]
    [HttpPatch("update-dietary-preferences/{id}")]
    public async Task<IActionResult> PatchDietaryPreferences(int id, [FromBody] RecipeDietaryPreferenceUpdateDto updateDto)
    {

        if (updateDto.DietaryPreferences == null || !updateDto.DietaryPreferences.Any())
            return BadRequest("Dietary preferences cannot be empty.");

        var userId = ClaimsHelper.GetUserIdFromClaims(User);

        var result = await _mediator.Send(new UpdateDietaryPreferencesCommand(id, userId, updateDto));

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("delete-recipe/{id}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var userId = ClaimsHelper.GetUserIdFromClaims(User);
        var userRole = ClaimsHelper.GetUserRoleFromClaims(User);

        var result = await _mediator.Send(new DeleteRecipeCommand(id, userId, userRole));

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/deleted-recipes")]
    public async Task<IActionResult> GetDeletedRecipes()
    {
        var result = await _mediator.Send(new GetDeletedRecipesQuery());

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/deleted-recipes/{id}")]
    public async Task<IActionResult> GetDeletedRecipe(int id)
    {
        var result = await _mediator.Send(new GetDeletedRecipeByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("admin/restore-recipe/{id}")]
    public async Task<IActionResult> RestoreDeletedRecipe(int id)
    {
        var result = await _mediator.Send(new RestoreDeletedRecipeCommand(id));

        if (!result.IsSuccess) 
            return NotFound(result.ErrorMessage);

        return Ok("Recipe successfully restored.");
    }
}