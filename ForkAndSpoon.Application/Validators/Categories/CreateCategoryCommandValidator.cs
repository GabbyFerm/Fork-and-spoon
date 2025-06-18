using FluentValidation;
using ForkAndSpoon.Application.Categorys.Commands.CreateCategory;

namespace ForkAndSpoon.Application.Validators.Categories
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            // Ensure the DTO is not null
            RuleFor(command => command.NewCategory)
                .NotNull()
                .WithMessage("Category data is required.");

            // Validate the category name
            RuleFor(command => command.NewCategory.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(50).WithMessage("Category name cannot be longer than 50 characters.")
                .Must(name => name.ToLower() != "string").WithMessage("Invalid default value.");
        }
    }
}