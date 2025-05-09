using FluentValidation;
using ForkAndSpoon.Application.Users.DTOs;

namespace ForkAndSpoon.Application.Validators
{
    public class UpdateEmailDtoValidator : AbstractValidator<UpdateEmailDto>
    {
        public UpdateEmailDtoValidator()
        {
            // Email must be valid and not empty
            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("A valid email is required.");
        }
    }
}
