using FluentValidation;
using ForkAndSpoon.Application.DTOs.Auth;

namespace ForkAndSpoon.Application.Validators
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidator()
        {
            RuleFor(user => user.UserName).NotEmpty().Length(3, 50);
            RuleFor(user => user.Email).NotEmpty().EmailAddress();
            RuleFor(user => user.Password).NotEmpty().MinimumLength(6);
        }
    }
}
