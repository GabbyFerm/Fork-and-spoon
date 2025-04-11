using ForkAndSpoon.Application.Identity.Auth;
using ForkAndSpoon.Application.Interfaces;
using MediatR;

namespace ForkAndSpoon.Application.Identity.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, string>
    {
        private readonly IAuthRepository _authRepository;

        public LoginQueryHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var dto = new UserLoginDto 
            { 
                Email = request.Email,
                Password = request.Password,            
            };

            return await _authRepository.LoginAsync(dto);
        }
    }
}
