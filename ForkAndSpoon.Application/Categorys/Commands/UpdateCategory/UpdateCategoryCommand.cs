using ForkAndSpoon.Application.Categorys.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<OperationResult<CategoryDto>>
    {
        public int CategoryID { get; }
        public string Name { get; }

        public UpdateCategoryCommand(int categoryId, string name)
        {
            CategoryID = categoryId;
            Name = name;
        }
    }
}