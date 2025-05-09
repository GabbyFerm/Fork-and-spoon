using AutoMapper;
using ForkAndSpoon.Application.Categorys.DTOs;
using ForkAndSpoon.Application.Interfaces.Generic;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, OperationResult<CategoryDto>>
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(IGenericRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Map the input DTO to a Category entity
                var categoryToCreate = _mapper.Map<Category>(request.NewCategory);

                // Try saving to database
                var result = await _categoryRepository.CreateAsync(categoryToCreate);

                // If successful, map to DTO and return success
                if (result.IsSuccess && result.Data != null)
                    return OperationResult<CategoryDto>.Success(_mapper.Map<CategoryDto>(result.Data));

                // Otherwise return the error
                return OperationResult<CategoryDto>.Failure(result.ErrorMessage!);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<CategoryDto>.Failure($"Error creating category: {ex.Message}");
            }
        }
    }
}
