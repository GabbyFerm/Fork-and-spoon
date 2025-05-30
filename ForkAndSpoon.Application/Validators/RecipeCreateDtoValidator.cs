using FluentValidation;
using ForkAndSpoon.Application.Recipes.DTOs;

namespace ForkAndSpoon.Application.Recipes.Validators
{
    public class RecipeCreateDtoValidator : AbstractValidator<RecipeCreateDto>
    {
        public RecipeCreateDtoValidator()
        {
            RuleFor(recipe => recipe.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must be at most 100 characters.");

            RuleFor(recipe => recipe.Steps)
                .NotEmpty().WithMessage("Steps are required.");

            RuleFor(recipe => recipe.CategoryID)
                .NotNull().WithMessage("A valid category must be selected.")
                .GreaterThan(0).WithMessage("Category ID must be greater than 0.");

            RuleFor(recipe => recipe.DietaryPreferences)
                .NotNull().WithMessage("Dietary preferences are required.");

            RuleForEach(recipe => recipe.DietaryPreferences)
                .NotEmpty().WithMessage("Each dietary preference must not be empty.");

            RuleForEach(recipe => recipe.Ingredients).ChildRules(ingredient =>
            {
                ingredient.RuleFor(i => i.Name)
                    .NotEmpty().WithMessage("Ingredient name is required.");

                ingredient.RuleFor(i => i.Quantity)
                    .NotEmpty().WithMessage("Ingredient quantity is required.");
            });
        }
    }
}
