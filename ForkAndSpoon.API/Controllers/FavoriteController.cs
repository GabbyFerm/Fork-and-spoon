using ForkAndSpoon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ForkAndSpoon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet("my-favorites")]
        public async Task<IActionResult> GetUserFavorites()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User token is missing.");

            int userId = int.Parse(userIdClaim.Value);

            var favorites = await _favoriteService.GetUserFavoritesAsync(userId);
            return Ok(favorites);
        }

        [HttpPost("add-favorite/{recipeId}")]
        public async Task<IActionResult> AddToFavorites(int recipeId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User token is missing.");

            int userId = int.Parse(userIdClaim.Value);

            try
            {
                var added = await _favoriteService.AddFavoriteAsync(userId, recipeId);
                if (!added)
                    return BadRequest("This recipe is already in your favorites.");

                return Ok("Recipe added to favorites.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("remove-favorite/{recipeId}")]
        public async Task<IActionResult> RemoveFromFavorites(int recipeId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User token is missing.");

            int userId = int.Parse(userIdClaim.Value);

            var wasRemoved = await _favoriteService.RemoveFavoriteAsync(userId, recipeId);
            if (!wasRemoved)
                return NotFound("Favorite not found.");

            return Ok("Recipe removed from favorites.");
        }
    }
}