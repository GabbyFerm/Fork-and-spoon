using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Application.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ForkAndSpoonDbContext _context;

        public AuthRepository(ForkAndSpoonDbContext context)
        {
            _context = context;
        }

        // Get a user by email (used in login or password reset)
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == email.ToLower());
        }

        // Check if an email is already registered
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(user => user.Email.ToLower() == email.ToLower());
        }

        // Add a new user to the database (save is called separately)
        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        // Save changes to the database
        public async Task<OperationResult<bool>> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<bool>.Failure($"Saving changes failed: {ex.Message}");
            }
        }
    }
}
