using FluentValidation;
using FluentValidation.AspNetCore;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Services;
using ForkAndSpoon.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace ForkAndSpoon.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // AutoMapper & Validators
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            services.AddValidatorsFromAssemblyContaining<UserRegisterDtoValidator>();
            services.AddFluentValidationAutoValidation();

            // Application Services
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFavoriteService, FavoriteService>();

            // External API
            services.AddHttpClient<ITriviaService, TriviaService>();
            return services;
        }
    }
}