using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Users.Commands.UpdateEmail
{
    public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand, OperationResult<bool>>
    {
        private readonly IUserRepository _userRepository;

        public UpdateEmailCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<bool>> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
        {
            // Fetch user by ID
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
                return OperationResult<bool>.Failure("User not found.");

            // Check for email duplication
            var emailExists = await _userRepository.EmailExistsAsync(request.NewEmail);
            if (emailExists)
                return OperationResult<bool>.Failure("Email is already registered.");

            // Update email and save
            user.Email = request.NewEmail;
            await _userRepository.SaveChangesAsync();

            // Return success
            return OperationResult<bool>.Success(true);
        }
    }
}