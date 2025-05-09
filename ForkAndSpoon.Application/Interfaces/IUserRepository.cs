using ForkAndSpoon.Application.Interfaces.Generic;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface IUserRepository : IReadRepository<User>
    {
        // Fetches a user by ID (basic lookup)
        Task<User?> GetUserByIdAsync(int userId);

        // Loads user with related data (Favorites + Ratings), used for deletion
        Task<User?> GetUserWithRelationsAsync(int userId);

        // Checks if a username is already taken (excluding the current user)
        Task<bool> UserNameExistsAsync(string newUserName, int excludeUserId);

        // Checks if an email is already registered
        Task<bool> EmailExistsAsync(string newEmail);

        // Saves all pending changes to the database
        Task<OperationResult<bool>> SaveChangesAsync();

        // Removes favorite recipes for a user (helper for deletion)
        void RemoveFavorites(IEnumerable<FavoriteRecipe> favorites);

        // Removes recipe ratings for a user (helper for deletion)
        void RemoveRatings(IEnumerable<Rating> ratings);

        // Removes the user from the database
        void RemoveUser(User user);
    }
}
