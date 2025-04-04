using AutoMapper;
using ForkAndSpoon.Application.DTOs.Category;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
        }
    }
}