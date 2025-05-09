using ForkAndSpoon.Application.Ingredients.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Ingredients.Commands.UpdateIngredient
{
    public class UpdateIngredientCommand : IRequest<OperationResult<IngredientReadDto>>
    {
        public int IngredientId { get; set; }
        public string UpdatedName { get; set; }

        public UpdateIngredientCommand(int ingredientId, string updatedName)
        {
            IngredientId = ingredientId;
            UpdatedName = updatedName;
        }
    }
}
