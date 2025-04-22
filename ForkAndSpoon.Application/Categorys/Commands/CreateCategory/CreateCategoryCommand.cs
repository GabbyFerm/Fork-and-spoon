using ForkAndSpoon.Application.Categorys.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<OperationResult<CategoryDto>>
    {
        public CategoryInputDto NewCategory { get; }

        public CreateCategoryCommand(CategoryInputDto newCategory)
        {
            NewCategory = newCategory;
        }
    }
}
