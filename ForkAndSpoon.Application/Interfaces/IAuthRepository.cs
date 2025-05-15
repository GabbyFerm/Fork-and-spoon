using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface IAuthRepository
    {
        // Retrieves a user by their username.
        Task<User?> GetUserByUsernameAsync(string username);

        // Retrieves a user by their email.
        Task<User?> GetUserByEmailAsync(string email);

        // Checks if an email is already registered in the system.
        Task<bool> EmailExistsAsync(string email);

        // Adds a new user to the database (registration).
        Task CreateUserAsync(User user);

        // Saves pending changes to the database and wraps the result.
        Task<OperationResult<bool>> SaveChangesAsync();
    }
}
