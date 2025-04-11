using ForkAndSpoon.Application.Interfaces;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Commands.RestoreDeletedRecipe
{
    public class RestoreDeletedRecipeCommandHandler : IRequestHandler<RestoreDeletedRecipeCommand, bool>
    {
        private readonly IRecipeRepository _recipeRepository;

        public RestoreDeletedRecipeCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<bool> Handle(RestoreDeletedRecipeCommand request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.RestoreDeletedRecipeAsync(request.RecipeId);
        }
    }
}
