using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Users.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Users.Queries.GetLoggedInUser
{
    public class GetLoggedInUserQueryHandler : IRequestHandler<GetLoggedInUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public GetLoggedInUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            return user!;
        }
    }
}
