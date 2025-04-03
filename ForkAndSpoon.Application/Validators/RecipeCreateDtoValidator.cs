using FluentValidation;
using ForkAndSpoon.Application.DTOs.Recipe;

namespace ForkAndSpoon.Application.Validators
{
    public class RecipeCreateDtoValidator : AbstractValidator<RecipeCreateDto>
    {
        public RecipeCreateDtoValidator()
        {
            RuleFor(recipe => recipe.Title).NotEmpty().MaximumLength(100);
            RuleFor(recipe => recipe.Steps).NotEmpty();
            RuleFor(recipe => recipe.CategoryID).GreaterThan(0).WithMessage("Please select a valid category.");
            RuleFor(recipe => recipe.Ingredients).NotEmpty().WithMessage("At least one ingredient is required.");
        }
    }
}
