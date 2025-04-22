using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OperationResult<bool>>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // Only allow users to delete themselves, or allow Admins to delete anyone
            if (request.TargetUserId != request.CallerUserId && request.CallerRole == "Admin")
            {
                return OperationResult<bool>.Failure("Not authorized to delete this user.");
            }

            return await _userRepository.DeleteUserAsync(request.TargetUserId);
        }
    }
}
