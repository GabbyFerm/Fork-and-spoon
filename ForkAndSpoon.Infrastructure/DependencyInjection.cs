﻿using ForkAndSpoon.Application.ExternalApi;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Services;
using ForkAndSpoon.Infrastructure.Database;
using ForkAndSpoon.Infrastructure.Repositories;
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
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();

            // External API
            services.AddHttpClient<ITriviaService, TriviaService>();

            return services;
        }
    }
}