using FluentValidation;
using ForkAndSpoon.Application.Authorize.DTOs;

namespace ForkAndSpoon.Application.Validators
{
    public class RegisterCommandValidator : AbstractValidator<UserRegisterDto>
    {
        public RegisterCommandValidator()
        {
            // Username must be provided and within valid length
            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.")
                .Must(name => name.ToLower() != "string").WithMessage("Invalid default value for username.");


            // Email must be valid and not empty
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .Must(email => email.ToLower() != "string").WithMessage("Invalid default value for email.");


            // Password must be strong and meet requirements
            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one number.")
                .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\+]").WithMessage("Password must contain at least one special character.")
                .Must(password => password.ToLower() != "string").WithMessage("Invalid default value for password.");

        }
    }
}
