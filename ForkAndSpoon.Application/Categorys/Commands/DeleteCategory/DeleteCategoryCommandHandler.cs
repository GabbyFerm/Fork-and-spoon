using ForkAndSpoon.Application.Interfaces.Generic;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, OperationResult<bool>>
    {
        private readonly IGenericRepository<Category> _categoryRepository;

        public DeleteCategoryCommandHandler(IGenericRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<OperationResult<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get category by ID
                var existingCategoryResult = await _categoryRepository.GetByIdAsync(request.CategoryID);

                if (!existingCategoryResult.IsSuccess || existingCategoryResult.Data == null)
                    return OperationResult<bool>.Failure("Category not found.");

                var categoryToDelete = existingCategoryResult.Data;

                // Only allow admins to delete the 'Uncategorized' category
                if (categoryToDelete.Name.Equals("Uncategorized", StringComparison.OrdinalIgnoreCase))
                {
                    return OperationResult<bool>.Failure("Default category cannot be deleted.");
                }

                // Proceed to delete
                return await _categoryRepository.DeleteAsync(categoryToDelete);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<bool>.Failure($"Error deleting category: {ex.Message}");
            }
        }
    }
}