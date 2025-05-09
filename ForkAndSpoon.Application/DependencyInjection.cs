using FluentValidation;
using FluentValidation.AspNetCore;
using ForkAndSpoon.Application.Interfaces.Services;
using ForkAndSpoon.Application.Services;
using ForkAndSpoon.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace ForkAndSpoon.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            // Register all MediatR handlers (Commands & Queries)
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

            // Register AutoMapper profiles
            services.AddAutoMapper(assembly);

            // Register all FluentValidation validators in the Application layer
            services.AddValidatorsFromAssemblyContaining<UserRegisterDtoValidator>();
            services.AddFluentValidationAutoValidation();

            // Register custom application-layer services
            services.AddScoped<IIngredientService, IngredientService>();

            return services;
        }
    }
}