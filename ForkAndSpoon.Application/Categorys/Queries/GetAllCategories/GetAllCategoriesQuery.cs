using ForkAndSpoon.Application.Categorys.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<OperationResult<List<CategoryDto>>>
    {
        // Could add extra logic here if needed
    }
}
