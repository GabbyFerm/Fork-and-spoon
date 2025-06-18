using FluentValidation;
using ForkAndSpoon.Application.Users.Commands.UpdateUserName;

namespace ForkAndSpoon.Application.Validators.Users
{
    public class UpdateUserNameCommandValidator : AbstractValidator<UpdateUserNameCommand>
    {
        public UpdateUserNameCommandValidator()
        {
            // Username is required and must be at least 3 characters
            RuleFor(user => user.NewUserName)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.")
                .Must(name => name.ToLower() != "string").WithMessage("Invalid default value for username.");
        }
    }
}