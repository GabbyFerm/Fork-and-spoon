using Microsoft.AspNetCore.Mvc;
using ForkAndSpoon.Application.DTOs.Auth;
using ForkAndSpoon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ForkAndSpoon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            var token = await _authService.RegisterAsync(registerDto);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            var token = await _authService.LoginAsync(loginDto);
            return Ok(token);
        }
    }
}
