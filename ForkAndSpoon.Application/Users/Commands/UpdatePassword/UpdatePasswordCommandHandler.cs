using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Users.Commands.UpdatePassword
{
    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, OperationResult<bool>>
    {
        private readonly IUserRepository _userRepository;

        public UpdatePasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<bool>> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            return await _userRepository.UpdatePasswordAsync(
                request.UserId,
                request.CurrentPassword,
                request.NewPassword
            );
        }
    }
}