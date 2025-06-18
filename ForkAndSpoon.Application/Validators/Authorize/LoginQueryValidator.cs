using FluentValidation;
using ForkAndSpoon.Application.Authorize.Queries;

namespace ForkAndSpoon.Application.Validators.Autorize
{
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(query => query.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .Must(name => name.ToLower() != "string").WithMessage("Invalid default value.");

            RuleFor(query => query.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
