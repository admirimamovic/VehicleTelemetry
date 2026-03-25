using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VehicleTelemetry.IoC.Middlewares;

namespace VehicleTelemetry.IoC;

public static class DependencyContainer
{
    public static IServiceCollection AddApplicationDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDatabaseConfiguration(configuration)
            .AddApplicationServices()
            .AddBackgroundServices()
            .AddSwaggerDocumentation()
            .AddHealthChecks();

        return services;
    }

    public static ILoggingBuilder ConfigureApplicationLogging(
        this ILoggingBuilder logging,
        IConfiguration configuration)
    {
        logging.AddCustomLogging(configuration);

        return logging;
    }

    public static WebApplication ConfigureApplicationPipeline(this WebApplication app)
    {
        app.ConfigureRequestPipeline();

        return app;
    }
}