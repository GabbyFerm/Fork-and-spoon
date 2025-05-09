using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Helpers
{
    public static class RecipeQueryHelper
    {
        public static IQueryable<Recipe> ApplyCategoryFilter(IQueryable<Recipe> query, string? categoryFilter)
        {
            // Filter by category name (case-insensitive)
            if (!string.IsNullOrWhiteSpace(categoryFilter))
            {
                query = query.Where(recipe =>
                    recipe.Category != null &&
                    recipe.Category.Name.ToLower() == categoryFilter.ToLower());
            }
            return query;
        }

        public static IQueryable<Recipe> ApplyDietaryPreferenceFilter(IQueryable<Recipe> query, string? dietaryPreferenceFilter)
        {
            // Filters by dietary preference name(case -insensitive)
            if (!string.IsNullOrWhiteSpace(dietaryPreferenceFilter))
            {
                query = query.Where(recipe =>
                    recipe.RecipeDietaryPreferences.Any(preference =>
                        preference.DietaryPreference.Name.ToLower() == dietaryPreferenceFilter.ToLower()));
            }
            return query;
        }

        public static IQueryable<Recipe> ApplySorting(IQueryable<Recipe> query, string? sortOrder)
        {
            // Applies sorting based on the sortOrder input
            return sortOrder?.ToLower() switch
            {
                "title_desc" => query.OrderByDescending(recipe => recipe.Title),
                "title_asc" => query.OrderBy(recipe => recipe.Title),
                _ => query.OrderBy(recipe => recipe.Title)
            };
        }

        public static IQueryable<Recipe> ApplyPagination(IQueryable<Recipe> query, int page, int pageSize)
        {
            // Applies pagination based on current page and page size
            int skip = (page - 1) * pageSize;
            return query.Skip(skip).Take(pageSize);
        }

        public static async Task<IQueryable<Recipe>> ApplyIngredientFilterAsync(IQueryable<Recipe> query, string? ingredientFilter, IIngredientRepository ingredientRepository)
        {
            // Filters the query by ingredient name (case-insensitive, async)
            if (!string.IsNullOrWhiteSpace(ingredientFilter))
            {
                var ingredientResult = await ingredientRepository.GetByNameAsync(ingredientFilter);

                // If not found or failure, return empty result
                if (!ingredientResult.IsSuccess || ingredientResult.Data == null)
                    return Enumerable.Empty<Recipe>().AsQueryable();

                var ingredientId = ingredientResult.Data.IngredientID;

                query = query.Where(recipe =>
                    recipe.RecipeIngredients.Any(ri => ri.IngredientID == ingredientId));
            }

            return query;
        }
    }
}