using ForkAndSpoon.API.Helpers;
using ForkAndSpoon.Application.Interfaces;
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
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    public async Task<ActionResult<List<RecipeReadDto>>> GetAllRecipes(
    [FromQuery] string? category,
    [FromQuery] string? ingredient,
    [FromQuery] string? dietary,
    [FromQuery] string? sortOrder,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
    {
        var query = new GetAllRecipesQuery(category, ingredient, dietary, sortOrder, page, pageSize);
        var recipes = await _mediator.Send(query);

        if (recipes == null || recipes.Count == 0)
            return NotFound("No recipes found with the given filters.");

        return Ok(recipes);
    }

    [HttpGet("get-recipe/{id}")]
    public async Task<ActionResult<RecipeReadDto>> GetRecipeById(int id)
    {
        var recipe = await _mediator.Send(new GetRecipeByIdQuery(id));

        if (recipe == null)
            return NotFound("Recipe not found.");

        return Ok(recipe);
    }

    [Authorize]
    [HttpPost("create-recipe")]
    public async Task<ActionResult<RecipeReadDto>> CreateRecipe([FromBody] RecipeCreateDto recipeToCreate)
    {
        var userId = ClaimsHelper.GetUserIdFromClaims(User);

        var command = new CreateRecipeCommand(recipeToCreate, userId);

        var createdRecipe = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetRecipeById), new { id = createdRecipe.RecipeID }, createdRecipe);
    }

    [Authorize]
    [HttpPut("update-recipe/{id}")]
    public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeUpdateDto updatedRecipe)
    {
        var userId = ClaimsHelper.GetUserIdFromClaims(User);
        var role = ClaimsHelper.GetUserRoleFromClaims(User);

        var command = new UpdateRecipeCommand(id, updatedRecipe, userId, role);
        var recipe = await _mediator.Send(command);

        if (recipe == null)
            return NotFound("Recipe not found, deleted, or not editable by this user.");

        return Ok(recipe);
    }

    [Authorize]
    [HttpPatch("update-dietary-preferences/{id}")]
    public async Task<IActionResult> PatchDietaryPreferences(int id, [FromBody] RecipeDietaryPreferenceUpdateDto updateDto)
    {
        var userId = ClaimsHelper.GetUserIdFromClaims(User);

        if (updateDto.DietaryPreferences == null || !updateDto.DietaryPreferences.Any())
            return BadRequest("Dietary preferences cannot be empty.");

        var command = new UpdateDietaryPreferencesCommand(id, userId, updateDto);

        var updatedRecipe = await _mediator.Send(command);

        if (updatedRecipe == null)
            return NotFound("Recipe not found or not owned by user.");

        return Ok(updatedRecipe);
    }

    [Authorize]
    [HttpDelete("delete-recipe/{id}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var userId = ClaimsHelper.GetUserIdFromClaims(User);
        var role = ClaimsHelper.GetUserRoleFromClaims(User);

        var command = new DeleteRecipeCommand(id, userId, role);

        var isDeleted = await _mediator.Send(command);

        if (!isDeleted)
            return NotFound("Recipe not found or not owned by user.");

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/deleted-recipes")]
    public async Task<IActionResult> GetDeletedRecipes()
    {
        var deletedRecipes = await _mediator.Send(new GetDeletedRecipesQuery());

        return Ok(deletedRecipes);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/deleted-recipes/{id}")]
    public async Task<IActionResult> GetDeletedRecipe(int id)
    {
        var recipe = await _mediator.Send(new GetDeletedRecipeByIdQuery(id));

        if (recipe == null) 
            return NotFound("Deleted recipe not found.");

        return Ok(recipe);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("admin/restore-recipe/{id}")]
    public async Task<IActionResult> RestoreDeletedRecipe(int id)
    {
        var result = await _mediator.Send(new RestoreDeletedRecipeCommand(id));

        if (!result) 
            return NotFound("Recipe not found or not deleted.");

        return Ok("Recipe successfully restored.");
    }
}