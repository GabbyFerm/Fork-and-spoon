using ForkAndSpoon.Application.Users.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<OperationResult<List<UserDto>>>
    {
        // Could add extra logic here if needed
    }
}