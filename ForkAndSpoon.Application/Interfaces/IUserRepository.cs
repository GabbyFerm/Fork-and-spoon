using ForkAndSpoon.Application.Interfaces.Generic;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface IUserRepository : IReadRepository<User>
    {
        Task<OperationResult<bool>> UpdateEmailAsync(int userId, string newEmail);
        Task<OperationResult<bool>> UpdatePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<OperationResult<bool>> UpdateUserNameAsync(int userId, string newUserName);
        Task<OperationResult<bool>> DeleteUserAsync(int userId);
    }
}
