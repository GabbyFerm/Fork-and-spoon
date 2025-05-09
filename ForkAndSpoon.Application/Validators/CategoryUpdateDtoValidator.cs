using FluentValidation;
using ForkAndSpoon.Application.Categorys.DTOs;

namespace ForkAndSpoon.Application.Validators
{
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryInputDto>
    {
        public CategoryUpdateDtoValidator()
        {
            // Name is required and has a max length
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name cannot be longer than 50 characters");
        }
    }
}
