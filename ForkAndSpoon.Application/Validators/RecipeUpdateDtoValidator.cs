using FluentValidation;
using ForkAndSpoon.Application.Recipes.DTOs;

namespace ForkAndSpoon.Application.Validators
{
    public class RecipeUpdateDtoValidator : AbstractValidator<RecipeUpdateDto>
    {
        public RecipeUpdateDtoValidator()
        {
            RuleFor(recipe => recipe.Title).NotEmpty().MaximumLength(100);
            RuleFor(recipe => recipe.Steps).NotEmpty();
            RuleFor(recipe => recipe.CategoryID).GreaterThan(0);
            RuleFor(recipe => recipe.DietaryPreferences).NotNull();
        }
    }
}
