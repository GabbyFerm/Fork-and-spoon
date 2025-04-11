using ForkAndSpoon.Application.Interfaces;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            // Only allow deleting "Uncategorized" if role is Admin
            var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryID);

            if (category == null)
                return false;

            if (category.Name.Equals("Uncategorized", StringComparison.OrdinalIgnoreCase)
                && request.Role != "Admin")
                return false;

            return await _categoryRepository.DeleteCategoryAsync(request.CategoryID);
        }
    }

}