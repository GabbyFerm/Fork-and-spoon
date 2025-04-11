using ForkAndSpoon.Application.Interfaces;
using MediatR;

namespace ForkAndSpoon.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // Allow if user is admin or deleting their own account
            if (request.CallerRole == "Admin" || request.TargetUserId == request.CallerUserId)
            {
                return await _userRepository.DeleteUserAsync(request.TargetUserId);
            }

            // Otherwise deny
            return false;
        }
    }
}
