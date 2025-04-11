using ForkAndSpoon.Application.Recipes.DTOs;
using MediatR;

namespace ForkAndSpoon.Application.Recipes.Queries.GetAllRecipes
{
    public class GetAllRecipesQuery : IRequest<List<RecipeReadDto>>
    {
        public string? Category { get; set; }
        public string? Ingredient { get; set; }
        public string? Dietary { get; set; }
        public string? SortOrder { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetAllRecipesQuery(string? category, string? ingredient, string? dietary, string? sortOrder, int page, int pageSize)
        {
            Category = category;
            Ingredient = ingredient;
            Dietary = dietary;
            SortOrder = sortOrder;
            Page = page;
            PageSize = pageSize;
        }
    }
}
