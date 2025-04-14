using FluentValidation;
using ForkAndSpoon.Application.Users.DTOs;

namespace ForkAndSpoon.Application.Validators
{
    public class UpdateUserNameDtoValidator : AbstractValidator<UpdateUserNameDto>
    {
        public UpdateUserNameDtoValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.");
        }
    }
}
