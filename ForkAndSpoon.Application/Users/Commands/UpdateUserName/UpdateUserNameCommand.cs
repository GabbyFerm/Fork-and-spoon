using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Users.Commands.UpdateUserName
{
    public class UpdateUserNameCommand : IRequest<OperationResult<bool>>
    {
        public int UserId { get; }
        public string NewUserName { get; }

        public UpdateUserNameCommand(int userId, string newUserName)
        {
            UserId = userId;
            NewUserName = newUserName;
        }
    }
}
