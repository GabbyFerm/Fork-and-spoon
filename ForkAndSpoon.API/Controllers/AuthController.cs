using ForkAndSpoon.Application.Identity.Auth;
using ForkAndSpoon.Application.Identity.Commands;
using ForkAndSpoon.Application.Identity.DTOs;
using ForkAndSpoon.Application.Identity.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForkAndSpoon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            var command = new RegisterCommand(registerDto.UserName, registerDto.Email, registerDto.Password);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            var query = new LoginQuery(loginDto.Email, loginDto.Password);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return Unauthorized(result.ErrorMessage);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetDto)
        {
            var command = new ResetPasswordCommand(resetDto.Email, resetDto.NewPassword);

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return NoContent(); // 204 response if success and nothing to return
        }
    }
}
