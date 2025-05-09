using FluentValidation;
using ForkAndSpoon.Application.Recipes.DTOs;

namespace ForkAndSpoon.Application.Validators
{
    public class RecipeUpdateDtoValidator : AbstractValidator<RecipeUpdateDto>
    {
        public RecipeUpdateDtoValidator()
        {
            // Title is required and max 100 characters
            RuleFor(recipe => recipe.Title)
                .NotEmpty()
                .MaximumLength(100);

            // Steps must be provided
            RuleFor(recipe => recipe.Steps)
                .NotEmpty();

            // Category must be selected
            RuleFor(recipe => recipe.CategoryID)
                .GreaterThan(0);

            // Dietary preferences must be valid
            RuleFor(recipe => recipe.DietaryPreferences)
                .NotNull();

            // Each dietary preference must not be empty
            RuleForEach(recipe => recipe.DietaryPreferences)
                .NotEmpty()
                .WithMessage("Dietary preference cannot be empty.");

            // Validate each ingredient
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
