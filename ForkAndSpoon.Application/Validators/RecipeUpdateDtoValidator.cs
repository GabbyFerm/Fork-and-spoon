using FluentValidation;
using ForkAndSpoon.Application.Recipes.DTOs;

namespace ForkAndSpoon.Application.Validators
{
    public class RecipeUpdateDtoValidator : AbstractValidator<RecipeUpdateDto>
    {
        public RecipeUpdateDtoValidator()
        {
            // Title is required and max 100 characters
            RuleFor(dto => dto.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot be longer than 100 characters.");

            // Steps must be provided
            RuleFor(dto => dto.Steps)
                .NotEmpty().WithMessage("Steps are required.");

            // Category must be selected (greater than 0)
            RuleFor(dto => dto.CategoryID)
                .GreaterThan(0).WithMessage("A valid category must be selected.");

            // Dietary preferences list must not be null
            RuleFor(dto => dto.DietaryPreferences)
                .NotNull().WithMessage("Dietary preferences list is required.");

            // Each dietary preference must not be empty
            RuleForEach(dto => dto.DietaryPreferences)
                .NotEmpty().WithMessage("Dietary preference cannot be empty.");

            // Ingredients list must not be null
            RuleFor(dto => dto.Ingredients)
                .NotNull().WithMessage("Ingredients list is required.");

            // Validate each ingredient's name and quantity
            RuleForEach(dto => dto.Ingredients).ChildRules(ingredient =>
            {
                ingredient.RuleFor(i => i.Name)
                    .NotEmpty().WithMessage("Ingredient name is required.");

                ingredient.RuleFor(i => i.Quantity)
                    .NotEmpty().WithMessage("Ingredient quantity is required.");
            });
        }
    }
}
