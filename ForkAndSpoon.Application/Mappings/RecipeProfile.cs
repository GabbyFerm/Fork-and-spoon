using AutoMapper;
using ForkAndSpoon.Application.DTOs.Recipe;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Mappings
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<Recipe, RecipeReadDto>()
            .ForMember(destination => destination.CategoryName, options => options.MapFrom(recipe => recipe.Category != null ? recipe.Category.Name : "Unknown Category"))
            .ForMember(destination => destination.Ingredients,
                options => options.MapFrom(recipe => recipe.RecipeIngredients.Select(recipeIngredient => recipeIngredient.Ingredient.Name).ToList()))
            .ForMember(destination => destination.DietaryPreferences,
                options => options.MapFrom(recipe => recipe.RecipeDietaryPreferences.Select(recipeDietaryPreference => recipeDietaryPreference.DietaryPreference.Name).ToList()));

            CreateMap<RecipeCreateDto, Recipe>();
            CreateMap<RecipeUpdateDto, Recipe>();
        }
    }
}