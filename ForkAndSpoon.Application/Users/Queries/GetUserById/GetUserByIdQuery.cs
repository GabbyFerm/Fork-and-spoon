using ForkAndSpoon.Application.Users.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserDto?>
    {
        public int UserId { get; }

        public GetUserByIdQuery(int userId)
        {
            UserId = userId;
        }
    }
}
