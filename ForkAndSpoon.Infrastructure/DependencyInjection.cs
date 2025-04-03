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

            return services;
        }
    }
}