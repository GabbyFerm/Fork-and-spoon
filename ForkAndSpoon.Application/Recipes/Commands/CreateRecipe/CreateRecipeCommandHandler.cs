using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Recipes.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, RecipeReadDto>
    {
        private readonly IRecipeRepository _recipeRepository;

        public CreateRecipeCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipeReadDto> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.CreateRecipeAsync(request.RecipeToCreate, request.UserId);
        }
    }
}
