using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Users.Commands.UpdatePassword
{
    public class UpdatePasswordCommand : IRequest<OperationResult<bool>>
    {
        public int UserId { get; }
        public string CurrentPassword { get; }
        public string NewPassword { get; }

        public UpdatePasswordCommand(int userId, string currentPassword, string newPassword)
        {
            UserId = userId;
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }
    }
}
