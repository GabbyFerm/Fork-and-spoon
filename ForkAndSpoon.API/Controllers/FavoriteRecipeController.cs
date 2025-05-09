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

            var result = await _mediator.Send(new GetUserFavoritesQuery(userId));

            if (!result.IsSuccess) 
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("add-favorite/{recipeId}")]
        public async Task<IActionResult> AddToFavorites(int recipeId)
        {
            var userId = ClaimsHelper.GetUserIdFromClaims(User);

            var result = await _mediator.Send(new AddToFavoritesCommand(userId, recipeId));

            if (!result.IsSuccess) 
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("remove-favorite/{recipeId}")]
        public async Task<IActionResult> RemoveFromFavorites(int recipeId)
        {
            var userId = ClaimsHelper.GetUserIdFromClaims(User);

            var result = await _mediator.Send(new RemoveFromFavoritesCommand(userId, recipeId));

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }
    }
}