using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Ingredients.Commands.DeleteIngredient
{
    public class DeleteIngredientCommand : IRequest<OperationResult<bool>>
    {
        public int IngredientId { get; set; }

        public DeleteIngredientCommand(int ingredientId)
        {
            IngredientId = ingredientId;
        }
    }
}
