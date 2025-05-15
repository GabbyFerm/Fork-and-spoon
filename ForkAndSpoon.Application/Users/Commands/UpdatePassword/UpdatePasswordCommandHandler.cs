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
            // Fetch user by ID
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
                return OperationResult<bool>.Failure("User not found.");

            // Validate current password
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.Password);
            if (!isPasswordValid)
                return OperationResult<bool>.Failure("Incorrect current password.");

            // Update password with hashed new password
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _userRepository.SaveChangesAsync();

            // Return success result
            return OperationResult<bool>.Success(true);
        }
    }
}