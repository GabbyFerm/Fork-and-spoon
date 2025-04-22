using ForkAndSpoon.Application.Categorys.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<OperationResult<CategoryDto>>
    {
        public int CategoryID { get;  }

        public GetCategoryByIdQuery(int categoryId) 
        { 
            CategoryID = categoryId;
        }
    }
}