using AutoMapper;
using ForkAndSpoon.Application.Categorys.DTOs;
using ForkAndSpoon.Application.Interfaces.Generic;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, OperationResult<List<CategoryDto>>>
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandler(IGenericRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            // Fetch all categories from repository
            var result = await _categoryRepository.GetAllAsync();

            // If fetching failed, return error message
            if (!result.IsSuccess)
                return OperationResult<List<CategoryDto>>.Failure(result.ErrorMessage ?? "Failed to fetch categories.");

            // Map entities to DTOs
            var dtoList = _mapper.Map<List<CategoryDto>>(result.Data);

            // Return success with mapped DTO list
            return OperationResult<List<CategoryDto>>.Success(dtoList);
        }
    }
}