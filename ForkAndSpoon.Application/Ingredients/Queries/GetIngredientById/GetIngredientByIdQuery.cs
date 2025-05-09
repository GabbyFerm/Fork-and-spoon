using ForkAndSpoon.Application.Ingredients.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Ingredients.Queries.GetIngredientById
{
    public class GetIngredientByIdQuery : IRequest<OperationResult<IngredientReadDto>>
    {
        public int IngredientId { get; set; }

        public GetIngredientByIdQuery(int ingredientId)
        {
            IngredientId = ingredientId;
        }
    }
}
