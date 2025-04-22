using AutoMapper;
using ForkAndSpoon.Application.Categorys.DTOs;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Interfaces.Generic;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, OperationResult<CategoryDto>>
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(IGenericRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryResult = await _categoryRepository.GetByIdAsync(request.CategoryID);
            if (!categoryResult.IsSuccess || categoryResult.Data == null)
                return OperationResult<CategoryDto>.Failure("Category not found.");

            var category = categoryResult.Data;

            // Restrictin access to renamin 'Uncategorized' to only admin
            if (category.Name.Equals("Uncategorized", StringComparison.OrdinalIgnoreCase)
            && request.Role != "Admin")
            {
                return OperationResult<CategoryDto>.Failure("Only admins can update the 'Uncategorized' category.");
            }

            category.Name = request.Name;

            var updateResult = await _categoryRepository.UpdateAsync(category);
            return updateResult.IsSuccess
                ? OperationResult<CategoryDto>.Success(_mapper.Map<CategoryDto>(updateResult.Data))
            : OperationResult<CategoryDto>.Failure(updateResult.ErrorMessage!);
        }
    }

}