using Microsoft.OpenApi.Models;

namespace ForkAndSpoon.API.Helpers
{
    public static class SwaggerSetup
    {
        // Extension method to configure Swagger and JWT support
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // Basic info shown in Swagger UI
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ForkAndSpoon API",
                    Version = "v1",
                    Description = "API with Clean Architecture, CQRS, JWT, and OperationResult"
                });

                // Add support for JWT Bearer token in Swagger UI
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Paste only your token below. Swagger will add the 'Bearer' prefix automatically."
                });

                // Tell Swagger to use the Bearer token on protected endpoints
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Authorization",
                            In = ParameterLocation.Header
                        },
                        new List<string>() // No specific scopes
                    }
                });
            });

            return services;
        }
    }
}
