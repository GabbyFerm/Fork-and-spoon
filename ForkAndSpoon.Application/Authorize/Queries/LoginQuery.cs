using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Authorize.Queries
{
    public class LoginQuery : IRequest<OperationResult<User>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginQuery(string username, string password)
        {
            UserName = username;
            Password = password;
        }
    }
}
