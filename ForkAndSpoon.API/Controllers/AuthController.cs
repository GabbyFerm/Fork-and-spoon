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
            var token = await _mediator.Send(command);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            var query = new LoginQuery(loginDto.Email, loginDto.Password);
            var token = await _mediator.Send(query);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetDto)
        {
            var command = new ResetPasswordCommand(resetDto.Email, resetDto.NewPassword);

            var result = await _mediator.Send(command);

            if (!result)
                return NotFound("user with that email was not found.");

            return NoContent(); // 204 response if success and nothing to return
        }
    }
}
