using FluentValidation;
using ForkAndSpoon.Application.Categorys.Commands.UpdateCategory;

namespace ForkAndSpoon.Application.Validators.Categories
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            // Category ID must be a valid (non-zero) value
            RuleFor(category => category.CategoryID)
                .GreaterThan(0).WithMessage("Invalid category ID.");

            // Validate the category name
            RuleFor(category => category.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(50).WithMessage("Category name cannot be longer than 50 characters.")
                .Must(name => name.ToLower() != "string").WithMessage("Invalid default value.");
        }
    }
}
