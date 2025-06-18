using FluentValidation;
using ForkAndSpoon.Application.Authorize.Commands.ResetPassword;

namespace ForkAndSpoon.Application.Validators.Autorize
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            // Email must be present and valid (future: token instead)
            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.")
                .Must(email => email.ToLower() != "string").WithMessage("Invalid default value for email.");

            // Password strength requirements
            RuleFor(command => command.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("New password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("New password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("New password must contain at least one number.")
                .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\+]").WithMessage("New password must contain at least one special character.")
                .Must(pwd => pwd.ToLower() != "string").WithMessage("Invalid default value for password.");
        }
    }
}