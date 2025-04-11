using ForkAndSpoon.Application.Categorys.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<CategoryDto?>
    {
        public int CategoryID { get;  }

        public GetCategoryByIdQuery(int categoryId) 
        { 
            CategoryID = categoryId;
        }
    }
}