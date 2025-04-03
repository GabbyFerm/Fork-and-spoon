using ForkAndSpoon.Application.DTOs.User;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<bool> DeleteUserAsync(int userId);
    }
}
