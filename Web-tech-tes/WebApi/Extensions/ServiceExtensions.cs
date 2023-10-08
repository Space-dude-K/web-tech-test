using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;
using Repository;
using WebApi.ActionFilters;
using WebApi.ActionFilters.Role;
using WebApi.ActionFilters.User;

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
        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
        public static void ConfigureValidationsFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidateMediaTypeAttribute>();
            services.AddScoped<ValidateUserExistsAttribute>();
            services.AddScoped<ValidateUsersExistsAttribute>();
            services.AddScoped<ValidateRoleExistsAttribute>();
        }
    }
}