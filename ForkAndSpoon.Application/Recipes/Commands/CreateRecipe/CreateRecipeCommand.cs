using ForkAndSpoon.Application.Recipes.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommand : IRequest<RecipeReadDto>
    {
        public RecipeCreateDto RecipeToCreate { get; }
        public int UserId { get; }

        public CreateRecipeCommand(RecipeCreateDto recipeToCreate, int userId)
        {
            RecipeToCreate = recipeToCreate;
            UserId = userId;
        }
    }
}
