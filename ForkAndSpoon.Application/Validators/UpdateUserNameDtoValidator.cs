using FluentValidation;
using ForkAndSpoon.Application.Users.DTOs;

namespace ForkAndSpoon.Application.Validators
{
    public class UpdateUserNameDtoValidator : AbstractValidator<UpdateUserNameDto>
    {
        public UpdateUserNameDtoValidator()
        {
            // Username is required and must be at least 3 characters
            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.");
        }
    }
}
