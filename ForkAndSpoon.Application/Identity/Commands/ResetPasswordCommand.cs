using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Identity.Commands
{
    public class ResetPasswordCommand : IRequest<OperationResult<bool>>
    {
        public string Email { get; }
        public string NewPassword { get; }

        public ResetPasswordCommand(string email, string newPassword)
        {
            Email = email;
            NewPassword = newPassword;
        }
    }
}
