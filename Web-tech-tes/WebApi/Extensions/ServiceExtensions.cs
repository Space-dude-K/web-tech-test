using Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<WebApiDbContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                b => b.MigrationsAssembly("WebApi")
            ));
        }
    }
}