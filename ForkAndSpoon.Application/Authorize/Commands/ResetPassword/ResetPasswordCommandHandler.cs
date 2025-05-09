using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Authorize.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, OperationResult<bool>>
    {
        private readonly IAuthRepository _authRepository;

        public ResetPasswordCommandHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<OperationResult<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try 
            {
                // Look up the user by their email
                var user = await _authRepository.GetUserByEmailAsync(request.Email);

                // If not found return an error message
                if (user == null)
                    return OperationResult<bool>.Failure("No user found with that email.");

                // Hash and update the user's password
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                await _authRepository.SaveChangesAsync();

                // Return success result
                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<bool>.Failure($"Error resetting password: {ex.Message}");
            }
        }
    }
}
