using MediatR;

namespace ForkAndSpoon.Application.Users.Commands.UpdateUserName
{
    public class UpdateUserNameCommand : IRequest<bool>
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
