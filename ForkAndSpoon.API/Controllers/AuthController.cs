using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ForkAndSpoon.Application.Authorize.Commands.Register;
using ForkAndSpoon.Application.Authorize.Commands.ResetPassword;
using ForkAndSpoon.Application.Authorize.Queries;
using ForkAndSpoon.Application.Authorize.DTOs;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure.Helpers;
using AutoMapper;

namespace ForkAndSpoon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly JWTGenerator _jwtGenerator;
        private readonly IMapper _mapper;

        public AuthController(IMediator mediator, JWTGenerator jwtGenerator, IMapper mapper)
        {
            _mediator = mediator;
            _jwtGenerator = jwtGenerator;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            var command = new RegisterCommand(registerDto.UserName, registerDto.Email, registerDto.Password);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess || result.Data is null)
                return BadRequest(result);

            // Generate token
            var token = _jwtGenerator.GenerateToken(result.Data);

            var response = new AuthResponseDto
            {
                Token = token,
                User = _mapper.Map<UserDtoResponse>(result.Data)
            };

            return Ok(OperationResult<AuthResponseDto>.Success(response));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var query = new LoginQuery(loginDto.UserName, loginDto.Password);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess || result.Data is null)
                return Unauthorized(result);

            // Generate token
            var token = _jwtGenerator.GenerateToken(result.Data);

            var userDto = _mapper.Map<UserDtoResponse>(result.Data);

            var response = new AuthResponseDto
            {
                Token = token,
                User = userDto
            };

            return Ok(OperationResult<AuthResponseDto>.Success(response));
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
