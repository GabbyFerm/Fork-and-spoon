using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ForkAndSpoonDbContext _context;

        public UserRepository(ForkAndSpoonDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<User>>> GetAllAsync()
        {
            try
            {
                // Get all users from the database
                var users = await _context.Users.ToListAsync();

                // Return the list wrapped in a success result
                return OperationResult<List<User>>.Success(users);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<List<User>>.Failure($"Error fetching users: {ex.Message}.");
            }
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            // Fetch single user for updates
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User?> GetUserWithRelationsAsync(int userId)
        {
            // Load user along with related favorites and ratings
            return await _context.Users
                .Include(user => user.FavoriteRecipes)
                .Include(user => user.Ratings)
                .FirstOrDefaultAsync(user => user.UserID == userId);
        }

        public async Task<OperationResult<User>> GetByIdAsync(int id)
        {
            try
            {
                // Attempt to find user by ID
                var user = await _context.Users.FindAsync(id);

                // Return failure if not found
                if (user == null)
                    return OperationResult<User>.Failure("User not found.");

                // Otherwise return user in a success result
                return OperationResult<User>.Success(user);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<User>.Failure($"Error fetching user: {ex.Message}.");
            }
        }

        public async Task<bool> UserNameExistsAsync(string newUserName, int excludeUserId)
        {
            // Check if the username already exists (excluding the current user)
            return await _context.Users
                .AnyAsync(user => user.UserName.ToLower() == newUserName.ToLower() && user.UserID != excludeUserId);
        }

        public async Task<bool> EmailExistsAsync(string newEmail)
        {
            // Check if the email is already in use
            return await _context.Users
                .AnyAsync(user => user.Email.ToLower() == newEmail.ToLower());
        }

        public void RemoveFavorites(IEnumerable<FavoriteRecipe> favorites)
        {
            // Remove all favorite entries
            _context.FavoriteRecipes.RemoveRange(favorites);
        }

        public void RemoveRatings(IEnumerable<Rating> ratings)
        {
            // Remove all rating entries
            _context.Ratings.RemoveRange(ratings);
        }

        public void RemoveUser(User user)
        {
            // Remove the user from the database
            _context.Users.Remove(user);
        }

        public async Task<OperationResult<bool>> SaveChangesAsync()
        {
            try
            {
                // Save all changes to the database
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