using ForkAndSpoon.Application.Categorys.DTOs;
using ForkAndSpoon.Application.Interfaces;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }
    }
}