using AutoMapper;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Application.DTOs;
using ForkAndSpoon.Application.DTOs.Category;

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