using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ForkAndSpoon.Application.Authorize.Commands.Register;
using ForkAndSpoon.Application.Authorize.Commands.ResetPassword;
using ForkAndSpoon.Application.Authorize.Queries;
using ForkAndSpoon.Application.Authorize.DTOs;

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
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var query = new LoginQuery(loginDto.UserName, loginDto.Password);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return Unauthorized(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetDto)
        {
            var command = new ResetPasswordCommand(resetDto.Email, resetDto.NewPassword);

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result); 
        }
    }
}
