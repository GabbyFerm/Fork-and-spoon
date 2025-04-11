using FluentValidation;
using ForkAndSpoon.Application.Identity.DTOs;

namespace ForkAndSpoon.Application.Validators
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            // this should be changed later (production setup) to use tokens and secure links instead of email
            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(dto => dto.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("New password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("New password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("New password must contain at least one number.")
                .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\+]").WithMessage("New password must contain at least one special character.");
        }
    }
}