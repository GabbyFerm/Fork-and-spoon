using FluentValidation;
using ForkAndSpoon.Application.Recipes.Commands.UpdateRecipe;

namespace ForkAndSpoon.Application.Validators
{
    public class UpdateRecipeCommandValidator : AbstractValidator<UpdateRecipeCommand>
    {
        public UpdateRecipeCommandValidator()
        {
            // Validate the inner DTO using its own validator
            RuleFor(command => command.UpdatedRecipe)
                .NotNull().WithMessage("Updated recipe data is required.")
                .SetValidator(new RecipeUpdateDtoValidator());
        }
    }
}
