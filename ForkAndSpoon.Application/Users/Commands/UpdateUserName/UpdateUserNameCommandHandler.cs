using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Users.Commands.UpdateUserName
{
    public class UpdateUserNameCommandHandler : IRequestHandler<UpdateUserNameCommand, OperationResult<bool>>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserNameCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<bool>> Handle(UpdateUserNameCommand request, CancellationToken cancellationToken)
        {
            return await _userRepository.UpdateUserNameAsync(request.UserId, request.NewUserName);
        }
    }
}
