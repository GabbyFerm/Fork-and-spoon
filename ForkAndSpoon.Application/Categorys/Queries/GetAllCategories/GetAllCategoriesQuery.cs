using ForkAndSpoon.Application.Categorys.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<List<CategoryDto>>
    {
        // Could add extra logic here if needed
    }
}
