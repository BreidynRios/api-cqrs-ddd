using Elastic.Serilog.Sinks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApi.Middlewares;

namespace WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLoggerSeriLog(configuration);
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
        }

        public static void AddLoggerSeriLog(this IServiceCollection services, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Elasticsearch([new Uri(configuration["ServicesClients:ElasticSearchServices:Host"]!)])
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(logger, dispose: true);
            });
        }

        public static IHost MigrateDatabase<T>(this IHost host, IConfiguration configuration) where T : DbContext
        {
            if (!Convert.ToBoolean(configuration["ApplyMigration"]))
                return host;
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    logger.LogInformation("Start migrating for docker");
                    var db = services.GetRequiredService<T>();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            return host;
        }
    }
}
