using FluentValidation;
using ForkAndSpoon.Application.Recipes.DTOs;

namespace ForkAndSpoon.Application.Validators
{
    public class RecipeCreateDtoValidator : AbstractValidator<RecipeCreateDto>
    {
        public RecipeCreateDtoValidator()
        {
            // Title is required and max 100 characters
            RuleFor(recipe => recipe.Title)
                .NotEmpty()
                .MaximumLength(100);

            // Steps are required
            RuleFor(recipe => recipe.Steps)
                .NotEmpty();

            // Category must be selected
            RuleFor(recipe => recipe.CategoryID)
                .GreaterThan(0)
                .WithMessage("Please select a valid category.");

            // At least one ingredient is required
            RuleFor(recipe => recipe.Ingredients)
                .NotEmpty()
                .WithMessage("At least one ingredient is required.");

            // Validate each ingredient
            RuleForEach(recipe => recipe.Ingredients).ChildRules(ingredient =>
            {
                ingredient.RuleFor(i => i.Name)
                    .NotEmpty().WithMessage("Ingredient name is required.");

                ingredient.RuleFor(i => i.Quantity)
                    .NotEmpty().WithMessage("Ingredient quantity is required.");
            });

            // Validate dietary preferences
            RuleForEach(recipe => recipe.DietaryPreferences)
                .NotEmpty().WithMessage("Dietary preference cannot be empty.");
        }
    }
}
