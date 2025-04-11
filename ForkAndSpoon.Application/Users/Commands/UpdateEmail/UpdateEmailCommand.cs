using MediatR;

namespace ForkAndSpoon.Application.Users.Commands.UpdateEmail
{
    public class UpdateEmailCommand : IRequest<bool>
    {
        public int UserId { get; }
        public string NewEmail { get; }

        public UpdateEmailCommand(int userId, string newEmail)
        {
            UserId = userId;
            NewEmail = newEmail;
        }
    }
}