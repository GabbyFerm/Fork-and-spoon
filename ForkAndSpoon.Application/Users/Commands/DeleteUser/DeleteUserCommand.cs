using MediatR;

namespace ForkAndSpoon.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public int TargetUserId { get; }
        public int CallerUserId { get; }
        public string CallerRole { get; }

        public DeleteUserCommand(int targetUserId, int callerUserId, string callerRole)
        {
            TargetUserId = targetUserId;
            CallerUserId = callerUserId;
            CallerRole = callerRole;
        }
    }
}
