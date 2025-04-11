using ForkAndSpoon.Application.Categorys.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CategoryDto>
    {
        public string Name { get; set; } = null!;

        public CreateCategoryCommand(string name)
        {
            Name = name;
        }
    }

}
