using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ForkAndSpoon.API.Helpers
{
    public static class AuthenticationSetup
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Load JWT settings from appsettings.json
            var jwtSettings = configuration.GetSection("JwtSettings");
            var keyString = jwtSettings["Key"];
            if (string.IsNullOrWhiteSpace(keyString))
                throw new ArgumentException("JWT Key is missing in configuration");

            var key = Encoding.UTF8.GetBytes(keyString);

            // Configure authentication scheme to use JWT Bearer tokens
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    // Set token validation rules
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false, // Can be true if you want strict validation
                        ValidateAudience = false, // Can be true if you want strict validation
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };

                    // Customize error messages for unauthorized or forbidden requests
                    options.Events = new JwtBearerEvents
                    {
                        OnForbidden = async context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync("{\"message\": \"Access denied. Admins only.\"}");
                        },
                        OnChallenge = async context =>
                        {
                            context.HandleResponse(); // Prevent default challenge behavior
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync("{\"message\": \"Unauthorized. Please log in.\"}");
                        }
                    };
                });

            return services;
        }
    }
}
