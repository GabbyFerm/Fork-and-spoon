using ForkAndSpoon.Application.Users.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Users.Queries.GetLoggedInUser
{
    public class GetLoggedInUserQuery : IRequest<OperationResult<UserDto>>
    {
        public int UserId { get; }

        public GetLoggedInUserQuery(int userId)
        { 
            UserId = userId;
        }
    }
}
