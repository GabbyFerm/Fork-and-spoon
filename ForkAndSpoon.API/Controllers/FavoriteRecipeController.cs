using ForkAndSpoon.API.Helpers;
using ForkAndSpoon.Application.Favorites.Commands.AddToFavorites;
using ForkAndSpoon.Application.Favorites.Commands.RemoveFromFavorites;
using ForkAndSpoon.Application.Favorites.Queries.GetUserFavorites;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForkAndSpoon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoriteRecipeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FavoriteRecipeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("my-favorites")]
        public async Task<IActionResult> GetUserFavorites()
        {
            var userId = ClaimsHelper.GetUserIdFromClaims(User);

            var favorites = await _mediator.Send(new GetUserFavoritesQuery(userId));
            return Ok(favorites);
        }

        [HttpPost("add-favorite/{recipeId}")]
        public async Task<IActionResult> AddToFavorites(int recipeId)
        {
            var userId = ClaimsHelper.GetUserIdFromClaims(User);

            try
            {
                var added = await _mediator.Send(new AddToFavoritesQuery(userId, recipeId));

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
            var userId = ClaimsHelper.GetUserIdFromClaims(User);

            var command = new RemoveFromFavoritesCommand(userId, recipeId);
            var success = await _mediator.Send(command);

            if (!success)
                return NotFound("Favorite not found.");

            return Ok("Recipe removed from favorites.");
        }
    }
}