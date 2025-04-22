using ForkAndSpoon.Application.Identity.Auth;
using ForkAndSpoon.Application.Identity.DTOs;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure.Database;
using ForkAndSpoon.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Application.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ForkAndSpoonDbContext _context;
        private readonly JWTGenerator _jwtGenerator;

        public AuthRepository(ForkAndSpoonDbContext context, JWTGenerator jWTGenerator)
        {
            _context = context;
            _jwtGenerator = jWTGenerator;
        }

        public async Task<OperationResult<string>> LoginAsync(UserLoginDto loginDto) 
        { 
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password)) 
            { 
                return OperationResult<string>.Failure("Invalid credentials.");
            }

            var token = _jwtGenerator.JWTTokenGenerator(user);
            return OperationResult<string>.Success(token);
        }

        public async Task<OperationResult<string>> RegisterAsync(UserRegisterDto userRegisterDto) 
        { 
            // Check if email exists in db
            var emailExists = await _context.Users.AnyAsync(user  => user.Email == userRegisterDto.Email);
            if (emailExists) 
            {
                return OperationResult<string>.Failure("Email is already registered.");
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

            var token = _jwtGenerator.JWTTokenGenerator(newUser);
            return OperationResult<string>.Success(token);
        }
        public async Task<OperationResult<bool>> ResetPasswordAsync(ResetPasswordDto resetDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == resetDto.Email);
            if (user == null) 
            {
                return OperationResult<bool>.Failure("No user found with that email.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(resetDto.NewPassword);
            await _context.SaveChangesAsync();

            return OperationResult<bool>.Success(true);
        }
    }
}
