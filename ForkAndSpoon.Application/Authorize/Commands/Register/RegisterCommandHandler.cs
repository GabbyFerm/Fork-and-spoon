using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Authorize.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, OperationResult<string>>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtGenerator _jwtGenerator;

        public RegisterCommandHandler(IAuthRepository authRepository, IJwtGenerator jwtGenerator)
        {
            _authRepository = authRepository;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<OperationResult<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try 
            {
                // Check if the email is already registered
                var emailExists = await _authRepository.EmailExistsAsync(request.Email);

                if (emailExists)
                    return OperationResult<string>.Failure("Email is already registered.");

                // Hash the user's password before storing it
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                // Create a new user entity
                var user = new User
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    Password = hashedPassword,
                    Role = "User" // Default role
                };

                // Save the user to the database
                await _authRepository.CreateUserAsync(user);
                await _authRepository.SaveChangesAsync();

                // Generate a JWT token for the user
                var token = _jwtGenerator.GenerateToken(user);

                // Return the token wrapped in a success result
                return OperationResult<string>.Success(token);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<string>.Failure($"Error during registration: {ex.Message}");
            }
        }
    }
}