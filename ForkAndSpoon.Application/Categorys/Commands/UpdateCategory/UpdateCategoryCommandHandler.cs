using ForkAndSpoon.Application.Categorys.DTOs;
using ForkAndSpoon.Application.Interfaces;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto?>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryID);

            if (category == null) throw new Exception("Category not found");

            if (category.Name.Equals("Uncategorized", StringComparison.OrdinalIgnoreCase)
                && request.Role != "Admin")
            {
                throw new UnauthorizedAccessException("You are not allowed to update the 'Uncategorized' category.");
            }

            var categoryToUpdate = new CategoryUpdateDto { Name = request.Name };
            return await _categoryRepository.UpdateCategoryAsync(request.CategoryID, categoryToUpdate);
        }

    }

}