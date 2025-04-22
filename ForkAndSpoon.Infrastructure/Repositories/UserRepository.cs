using AutoMapper;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ForkAndSpoonDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ForkAndSpoonDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<User>>> GetAllAsync()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return OperationResult<List<User>>.Success(users);
            }
            catch (Exception ex)
            {
                return OperationResult<List<User>>.Failure($"Error fetching users: {ex.Message}");
            }
        }

        public async Task<OperationResult<User>> GetByIdAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return OperationResult<User>.Failure("User not found");

                return OperationResult<User>.Success(user);
            }
            catch (Exception ex)
            {
                return OperationResult<User>.Failure($"Error fetching user: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(user => user.FavoriteRecipes)
                    .Include(user => user.Ratings)
                    .FirstOrDefaultAsync(u => u.UserID == userId);

                if (user == null)
                    return OperationResult<bool>.Failure("User not found.");

                // Remove related favorites
                if (user.FavoriteRecipes.Any())
                    _context.FavoriteRecipes.RemoveRange(user.FavoriteRecipes);

                // Remove related ratings
                if (user.Ratings.Any())
                    _context.Ratings.RemoveRange(user.Ratings);

                // Remove the user
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Failed to delete user: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> UpdateEmailAsync(int userId, string newEmail)
        {
            try 
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return OperationResult<bool>.Failure("User not found.");

                // Check if the new email already exists in the database
                var emailExists = await _context.Users.AnyAsync(user => user.Email == newEmail);
                if (emailExists)
                    return OperationResult<bool>.Failure("Email is already registered.");

                // Update the user's email
                user.Email = newEmail;
                await _context.SaveChangesAsync();

                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Failed to update email: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> UpdatePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            try 
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return OperationResult<bool>.Failure("User not found.");

                // Verify the current password
                var isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(currentPassword, user.Password);
                if (!isCurrentPasswordValid)
                    return OperationResult<bool>.Failure("Incorrect current password.");

                // Hash the new password and update
                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await _context.SaveChangesAsync();

                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Failed to update password: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> UpdateUserNameAsync(int userId, string newUserName)
        {
            try 
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return OperationResult<bool>.Failure("User not found.");

                // Check if the new username already exists in the database
                var userNameExists = await _context.Users.AnyAsync(user => user.UserName.ToLower() == newUserName.ToLower() && user.UserID != userId);

                if (userNameExists)
                    return OperationResult<bool>.Failure("Username is already taken.");

                // Update the user's username
                user.UserName = newUserName;
                await _context.SaveChangesAsync();

                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Failed to update username: {ex.Message}");
            }
        }
    }
}