using ForkAndSpoon.Application.Ingredients.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Ingredients.Queries.GetIngredientByName
{
    public class GetIngredientByNameQuery : IRequest<OperationResult<IngredientReadDto>>
    {
        public string Name { get; set; }

        public GetIngredientByNameQuery(string name)
        {
            Name = name;
        }
    }
}
