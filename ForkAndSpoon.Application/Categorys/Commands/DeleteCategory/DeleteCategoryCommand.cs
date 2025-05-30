using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<OperationResult<bool>>
    {
        public int CategoryID { get; }

        public DeleteCategoryCommand(int categoryId)
        {
            CategoryID = categoryId;
        }
    }
}