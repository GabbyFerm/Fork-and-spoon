using ForkAndSpoon.Application.ExternalApi;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Interfaces.Generic;
using ForkAndSpoon.Application.Interfaces.Services;
using ForkAndSpoon.Application.Services;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure.Database;
using ForkAndSpoon.Infrastructure.Helpers;
using ForkAndSpoon.Infrastructure.Repositories;
using ForkAndSpoon.Infrastructure.Repositories.Generic;
using ForkAndSpoon.Infrastructure.Services;
using ForkAndSpoon.Infrastructure.Services.Trivia;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ForkAndSpoon.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ForkAndSpoonDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ForkSpoonDbConnection")));

            // Register Repositories here
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();

            // Services
            services.AddScoped<IDietaryPreferenceService, DietaryPreferenceService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IRecipeLoaderService, RecipeLoaderService>();

            // External API
            services.AddHttpClient<ITriviaService, TriviaService>();

            // JWT Generator
            services.AddScoped<IJwtGenerator, JWTGenerator>();

            return services;
        }
    }
}