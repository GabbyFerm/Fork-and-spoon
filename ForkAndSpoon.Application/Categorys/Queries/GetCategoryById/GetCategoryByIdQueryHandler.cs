using AutoMapper;
using ForkAndSpoon.Application.Categorys.DTOs;
using ForkAndSpoon.Application.Interfaces.Generic;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, OperationResult<CategoryDto>>
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IGenericRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<OperationResult<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            // Fetch category by ID
            var result = await _categoryRepository.GetByIdAsync(request.CategoryID);

            // If not found or failed, return failure
            if (!result.IsSuccess || result.Data == null)
                return OperationResult<CategoryDto>.Failure("Category not found.");

            // Map entity to DTO
            var dto = _mapper.Map<CategoryDto>(result.Data);

            // Return success with mapped DTO
            return OperationResult<CategoryDto>.Success(dto);
        }
    }
}