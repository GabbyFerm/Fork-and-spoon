using FluentValidation;
using FluentValidation.AspNetCore;
using ForkAndSpoon.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace ForkAndSpoon.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            // MediatR for CQRS
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

            // AutoMapper
            services.AddAutoMapper(assembly);

            // FluentValidation
            services.AddValidatorsFromAssemblyContaining<UserRegisterDtoValidator>();
            services.AddFluentValidationAutoValidation();

            return services;
        }
    }
}