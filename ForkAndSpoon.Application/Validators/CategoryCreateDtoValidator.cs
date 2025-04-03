using FluentValidation;
using ForkAndSpoon.Application.DTOs.Category;

namespace ForkAndSpoon.Application.Validators
{
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(category => category.Name)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Category name is required and must be less than 50 characters.");
        }
    }
}
