using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using WebApi.Extensions;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.ConfigureSqlContext(builder.Configuration);

            // Add identity types
            /*builder.Services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WebApiDbContext>()
                .AddDefaultTokenProviders();*/


            




            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}