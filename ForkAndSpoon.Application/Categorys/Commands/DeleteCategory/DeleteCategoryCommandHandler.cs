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
            var existingCategoryResult = await _categoryRepository.GetByIdAsync(request.CategoryID);

            if (!existingCategoryResult.IsSuccess || existingCategoryResult.Data == null)
                return OperationResult<bool>.Failure("Category not found.");

            var categoryToDelete = existingCategoryResult.Data;

            // Only allow deleting 'Uncategorized' if role is Admin
            if (categoryToDelete.Name.Equals("Uncategorized", StringComparison.OrdinalIgnoreCase) && request.Role != "Admin")
                return OperationResult<bool>.Failure("Only Admin can delete the 'Uncategorized' category.");

            return await _categoryRepository.DeleteAsync(categoryToDelete);
        }
    }


}