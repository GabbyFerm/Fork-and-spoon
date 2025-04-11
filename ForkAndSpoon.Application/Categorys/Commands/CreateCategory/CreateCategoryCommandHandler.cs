using ForkAndSpoon.Application.Categorys.DTOs;
using ForkAndSpoon.Application.Interfaces;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var createDto = new CategoryCreateDto
            {
                Name = request.Name
            };

            return await _categoryRepository.CreateCategoryAsync(createDto);
        }
    }

}
