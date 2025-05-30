using FluentValidation;
using ForkAndSpoon.Application.Users.Commands.UpdateEmail;

namespace ForkAndSpoon.Application.Validators
{
    public class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
    {
        public UpdateEmailCommandValidator()
        {
            // Email must be valid and not empty
            RuleFor(command => command.NewEmail)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("A valid email is required.")
                .Must(email => email.ToLower() != "string").WithMessage("Invalid default email value.");
        }
    }
}
