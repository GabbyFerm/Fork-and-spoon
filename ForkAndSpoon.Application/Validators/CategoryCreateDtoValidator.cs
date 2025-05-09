using FluentValidation;
using ForkAndSpoon.Application.Categorys.DTOs;

namespace ForkAndSpoon.Application.Validators
{
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryInputDto>
    {
        public CategoryCreateDtoValidator()
        {
            // Name is required and has a max length
            RuleFor(category => category.Name)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Category name is required and must be less than 50 characters.");
        }
    }
}
