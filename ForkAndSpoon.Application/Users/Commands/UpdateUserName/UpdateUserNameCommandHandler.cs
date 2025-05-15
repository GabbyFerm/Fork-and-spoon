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
            // Fetch user by ID
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
                return OperationResult<bool>.Failure("User not found.");

            // Check if username already exists (excluding current user)
            var userNameTaken = await _userRepository.UserNameExistsAsync(request.NewUserName, request.UserId);
            if (userNameTaken)
                return OperationResult<bool>.Failure("Username is already taken.");

            // Update username and save
            user.UserName = request.NewUserName;
            await _userRepository.SaveChangesAsync();

            // Return success result
            return OperationResult<bool>.Success(true);
        }
    }
}
