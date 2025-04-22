using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Identity.Commands
{
    public class RegisterCommand : IRequest<OperationResult<string>>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterCommand(string userName, string email, string password) 
        { 
            UserName = userName;
            Email = email;
            Password = password;
        }
    }
}