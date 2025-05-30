using FluentValidation;
using ForkAndSpoon.Application.Recipes.Commands.CreateRecipe;
using ForkAndSpoon.Application.Recipes.Validators;

namespace ForkAndSpoon.Application.Validators
{
    public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
    {
        public CreateRecipeCommandValidator()
        {
            RuleFor(command => command.RecipeToCreate)
                 .NotNull().WithMessage("Recipe data is required.")
                 .SetValidator(new RecipeCreateDtoValidator());
        }
    }
}
