using ForkAndSpoon.Application.Users.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Users.Queries.GetLoggedInUser
{
    public class GetLoggedInUserQuery : IRequest<UserDto>
    {
        public int UserId { get; }

        public GetLoggedInUserQuery(int userId)
        { 
            UserId = userId;
        }
    }
}
