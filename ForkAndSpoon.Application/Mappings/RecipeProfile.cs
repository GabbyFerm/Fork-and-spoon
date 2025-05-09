using AutoMapper;
using ForkAndSpoon.Application.Ingredients.DTOs;
using ForkAndSpoon.Application.Recipes.DTOs;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Mappings
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<Recipe, RecipeReadDto>()
            .ForMember(destination => destination.CategoryName,
                options => options.MapFrom(recipe =>
                    recipe.Category != null ? recipe.Category.Name : "Uncategorized"))

            .ForMember(destination => destination.Ingredients,
                options => options.MapFrom(recipe =>
                    recipe.RecipeIngredients.Select(ri => new IngredientWithQuantityDto
                    {
                        Name = ri.Ingredient.Name,
                        Quantity = ri.Quantity
                    }).ToList()))

            .ForMember(destination => destination.DietaryPreferences,
                options => options.MapFrom(recipe =>
                    recipe.RecipeDietaryPreferences.Select(rdp => rdp.DietaryPreference.Name).ToList()));

            CreateMap<RecipeCreateDto, Recipe>();
            CreateMap<RecipeUpdateDto, Recipe>();
        }
    }
}