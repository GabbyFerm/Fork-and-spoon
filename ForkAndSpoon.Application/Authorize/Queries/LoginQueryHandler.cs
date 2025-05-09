using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Authorize.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, OperationResult<string>>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtGenerator _jwtGenerator;

        public LoginQueryHandler(IAuthRepository authRepository, IJwtGenerator jwtGenerator)
        {
            _authRepository = authRepository;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<OperationResult<string>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            try 
            {
                // Look up the user by email
                var user = await _authRepository.GetUserByEmailAsync(request.Email);

                // Validate password
                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                    return OperationResult<string>.Failure("Invalid credentials.");

                // Generate a token and return it
                var token = _jwtGenerator.GenerateToken(user);

                // Return the token wrapped in a success result
                return OperationResult<string>.Success(token);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<string>.Failure($"Error during login: {ex.Message}");
            }
        }
    }
}