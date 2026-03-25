using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace VehicleTelemetry.IoC;

public static class LoggingConfiguration
{
    public static ILoggingBuilder AddCustomLogging(this ILoggingBuilder logging, IConfiguration configuration)
    {
        logging.ClearProviders();

        logging.AddConsole();

        logging.AddDebug();

        logging.AddConfiguration(configuration.GetSection("Logging"));

        return logging;
    }
}