using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Authorize.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, OperationResult<User>>
    {
        private readonly IAuthRepository _authRepository;

        public LoginQueryHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<OperationResult<User>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            try 
            {
                // Look up the user by email
                var user = await _authRepository.GetUserByUsernameAsync(request.UserName);

                // Validate password
                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                    return OperationResult<User>.Failure("Invalid credentials.");

                return OperationResult<User>.Success(user);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<User>.Failure($"Error during login: {ex.Message}");
            }
        }
    }
}