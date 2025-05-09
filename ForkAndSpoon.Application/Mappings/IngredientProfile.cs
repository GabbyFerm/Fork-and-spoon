using AutoMapper;
using ForkAndSpoon.Application.Ingredients.DTOs;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Mappings
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile() 
        { 
            CreateMap<Ingredient, IngredientReadDto>();
        }
    }
}
