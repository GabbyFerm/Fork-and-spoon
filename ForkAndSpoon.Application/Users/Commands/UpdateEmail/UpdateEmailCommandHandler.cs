using ForkAndSpoon.Application.Interfaces;
using MediatR;

namespace ForkAndSpoon.Application.Users.Commands.UpdateEmail
{
    public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UpdateEmailCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
        {
            return await _userRepository.UpdateEmailAsync(request.UserId, request.NewEmail);

        }
    }
}