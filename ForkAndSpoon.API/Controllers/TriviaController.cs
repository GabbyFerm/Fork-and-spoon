using ForkAndSpoon.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ForkAndSpoon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TriviaController : ControllerBase
    {
        private readonly ITriviaService _triviaService;

        public TriviaController(ITriviaService triviaService)
        {
            _triviaService = triviaService;
        }

        [HttpGet("random-trivia")]
        public async Task<IActionResult> GetRandomFoodTrivia()
        {
            var trivia = await _triviaService.GetRandomTriviaAsync();
            return Ok(trivia);
        }
    }
}
