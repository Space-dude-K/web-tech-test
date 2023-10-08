using NLog;
using NLog.Web;
using WebApi.Extensions;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Early init of NLog to allow startup and exception logging, before host is built
            var logger = LogManager
                .Setup()
                .LoadConfigurationFromAppSettings()
                .GetCurrentClassLogger();

            logger.Debug("Init main");

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Services.ConfigureSqlContext(builder.Configuration);
                builder.Services.AddAutoMapper(typeof(Program));
                builder.Services.ConfigureRepositoryManager();
                builder.Services.ConfigureValidationsFilters();

                builder.Services.AddControllers()
                    .AddNewtonsoftJson();

                // NLog: Setup NLog for Dependency injection
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                var app = builder.Build();

                // Configure the HTTP request pipeline.

                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
            // Ignore migrations
            catch(HostAbortedException) { }
            catch(Exception exception)
            {
                // NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            } 
        }
    }
}