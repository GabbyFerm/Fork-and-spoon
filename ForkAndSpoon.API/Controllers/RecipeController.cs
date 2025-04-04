using ForkAndSpoon.Application.DTOs.Recipe;
using ForkAndSpoon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipeController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [HttpGet("get-recipes")]
    public async Task<ActionResult<List<RecipeReadDto>>> GetAllRecipes([FromQuery] string? category, [FromQuery] string? ingredient, [FromQuery] string? dietary, [FromQuery] string? sortOrder, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var recipes = await _recipeService.GetAllRecipesAsync(category, ingredient, dietary, sortOrder, page, pageSize);

        if (recipes == null || recipes.Count == 0)
            return NotFound("No recipes found with the given filters.");

        return Ok(recipes);
    }

    [HttpGet("get-recipe/{id}")]
    public async Task<ActionResult<RecipeReadDto>> GetRecipeById(int id)
    {
        var recipe = await _recipeService.GetRecipeByIdAsync(id);

        if (recipe == null) 
            return NotFound("Recipe not found.");

        return Ok(recipe);
    }

    [Authorize]
    [HttpPost("create-recipe")]
    public async Task<ActionResult<RecipeReadDto>> CreateRecipe([FromBody] RecipeCreateDto recipeToCreate)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null) 
            return Unauthorized("Unable to identify the user from the authentication token.");

        int userId = int.Parse(userIdClaim.Value);

        var createdRecipe = await _recipeService.CreateRecipeAsync(recipeToCreate, userId);

        return CreatedAtAction(nameof(GetRecipeById), new { id = createdRecipe.RecipeID }, createdRecipe);
    }

    [Authorize]
    [HttpPut("update-recipe/{id}")]
    public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeUpdateDto updatedRecipe)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var roleClaim = User.FindFirst(ClaimTypes.Role);

        if (userIdClaim == null || roleClaim == null)
            return Unauthorized("Unable to extract user ID or role from token.");

        int userId = int.Parse(userIdClaim.Value);
        string role = roleClaim.Value;

        var recipe = await _recipeService.UpdateRecipeAsync(id, updatedRecipe, userId, role);

        if (recipe == null)
            return NotFound("Recipe not found, deleted, or not editable by this user.");

        return Ok(recipe);
    }

    [Authorize]
    [HttpPatch("update-dietary-preferences/{id}")]
    public async Task<IActionResult> PatchDietaryPreferences(int id, [FromBody] RecipeDietaryPreferenceUpdateDto updateDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) 
            return Unauthorized("Unable to identify the user from the authentication token.");

        if (updateDto.DietaryPreferences == null || !updateDto.DietaryPreferences.Any())
            return BadRequest("Dietary preferences cannot be empty.");

        int userId = int.Parse(userIdClaim.Value);

        var updatedRecipe = await _recipeService.UpdateDietaryPreferencesAsync(id, userId, updateDto);

        if (updatedRecipe == null)
            return NotFound("Recipe not found or not owned by user.");
        else
            return Ok(updatedRecipe);
    }

    [Authorize]
    [HttpDelete("delete-recipe/{id}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (userIdClaim == null) return Unauthorized("User ID claim is missing.");

        int userId = int.Parse(userIdClaim.Value);

        var isDeleted = await _recipeService.DeleteRecipeAsync(id, userId, role!);

        if (!isDeleted)
            return NotFound("Recipe not found or not owned by user.");

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/deleted-recipes")]
    public async Task<IActionResult> GetDeletedRecipes()
    {
        var deletedRecipes = await _recipeService.GetDeletedRecipesAsync();

        return Ok(deletedRecipes);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/deleted-recipes/{id}")]
    public async Task<IActionResult> GetDeletedRecipe(int id)
    {
        var recipe = await _recipeService.GetDeletedRecipeByIdAsync(id);

        if (recipe == null) 
            return NotFound("Deleted recipe not found.");

        return Ok(recipe);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("admin/restore-recipe/{id}")]
    public async Task<IActionResult> RestoreDeletedRecipe(int id)
    {
        var restored = await _recipeService.RestoreDeletedRecipeAsync(id);

        if (!restored) 
            return NotFound("Recipe not found or not deleted.");

        return Ok("Recipe successfully restored.");
    }
}