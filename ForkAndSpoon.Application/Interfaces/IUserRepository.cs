using ForkAndSpoon.Application.Users.DTOs;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> UpdateEmailAsync(int userId, string newEmail);
        Task<bool> UpdatePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> UpdateUserNameAsync(int userId, string newUserName);
    }
}
