using ForkAndSpoon.Application.Identity.Auth;
using ForkAndSpoon.Application.Interfaces;
using MediatR;

namespace ForkAndSpoon.Application.Identity.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly IAuthRepository _authRepository;

        public RegisterCommandHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var dto = new UserRegisterDto 
            { 
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password,
            
            };

            return await _authRepository.RegisterAsync(dto);
        }
    }
}