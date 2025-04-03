using ForkAndSpoon.Application.DTOs.Auth;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure;
using ForkAndSpoon.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ForkAndSpoonDbContext _context;
        private readonly JWTGenerator _jwtGenerator;

        public AuthService(ForkAndSpoonDbContext context, JWTGenerator jWTGenerator)
        {
            _context = context;
            _jwtGenerator = jWTGenerator;
        }

        public async Task<string> LoginAsync(UserLoginDto loginDto) 
        { 
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password)) 
            { 
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            return _jwtGenerator.JWTTokenGenerator(user);
        }

        public async Task<string> RegisterAsync(UserRegisterDto userRegisterDto) 
        { 
            // Check if email exists in db
            var emailExists = await _context.Users.AnyAsync(user  => user.Email == userRegisterDto.Email);
            if (emailExists) 
            {
                throw new InvalidOperationException("Email is already registered.");
            }
            
            // Hash password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password);

            var newUser = new User
            {
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.Email,
                Password = hashedPassword,
                Role = "User" // Default role
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return _jwtGenerator.JWTTokenGenerator(newUser);
        }
    }
}
