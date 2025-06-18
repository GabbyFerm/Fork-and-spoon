using FluentValidation;
using ForkAndSpoon.Application.Recipes.Commands.CreateRecipe;
using ForkAndSpoon.Application.Validators.Recipes;

namespace ForkAndSpoon.Application.Validators.Categories
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
