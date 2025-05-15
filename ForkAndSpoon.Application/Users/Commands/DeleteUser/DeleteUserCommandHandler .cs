using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OperationResult<bool>>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {

            // Authorization check: allow self-deletion or admin access
            if (request.TargetUserId != request.CallerUserId && request.CallerRole != "Admin")
                return OperationResult<bool>.Failure("Not authorized to delete this user.");

            // Fetch user including related favorites and ratings
            var user = await _userRepository.GetUserWithRelationsAsync(request.TargetUserId);
            if (user == null)
                return OperationResult<bool>.Failure("User not found.");

            // Remove related favorites if any
            if (user.FavoriteRecipes.Any())
                _userRepository.RemoveFavorites(user.FavoriteRecipes);

            // Remove related ratings if any
            if (user.Ratings.Any())
                _userRepository.RemoveRatings(user.Ratings);

            // Remove the user entity
            _userRepository.RemoveUser(user);

            // Commit changes to database
            await _userRepository.SaveChangesAsync();

            // Return success result
            return OperationResult<bool>.Success(true);
        }
    }
}
