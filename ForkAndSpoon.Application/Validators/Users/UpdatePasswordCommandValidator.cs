using FluentValidation;
using ForkAndSpoon.Application.Users.Commands.UpdatePassword;

namespace ForkAndSpoon.Application.Validators.Users
{
    public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
    {
        public UpdatePasswordCommandValidator()
        {
            RuleFor(command => command.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required.");

            RuleFor(command => command.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("New password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("New password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("New password must contain at least one number.")
                .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\+]").WithMessage("New password must contain at least one special character.")
                .Must(pwd => pwd.ToLower() != "string").WithMessage("Invalid default value for password.");

            RuleFor(command => command)
                .Must(password => password.CurrentPassword != password.NewPassword)
                .WithMessage("New password must be different from the current password.");
        }
    }
}
