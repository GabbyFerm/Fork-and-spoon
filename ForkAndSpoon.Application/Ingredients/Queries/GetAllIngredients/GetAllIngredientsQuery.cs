using ForkAndSpoon.Application.Ingredients.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Ingredients.Queries.GetAllIngredients
{
    public class GetAllIngredientsQuery : IRequest<OperationResult<List<IngredientReadDto>>> 
    {
        // Could add extra logic here if needed
    }
}
