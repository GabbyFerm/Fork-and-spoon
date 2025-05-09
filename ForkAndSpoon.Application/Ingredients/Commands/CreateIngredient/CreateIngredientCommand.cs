using ForkAndSpoon.Application.Ingredients.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Ingredients.Commands.CreateIngredient
{
    public class CreateIngredientCommand : IRequest<OperationResult<IngredientReadDto>>
    {
        public string Name { get; set; }

        public CreateIngredientCommand(string name)
        {
            Name = name;
        }
    }
}
