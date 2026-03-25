using Microsoft.Extensions.DependencyInjection;
using VehicleTelemetry.BL.Services.BackgroundServices;

namespace VehicleTelemetry.IoC;

public static class BackgroundServiceConfiguration
{
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        // Register Background/Hosted Services
        services.AddHostedService<CloudUploadService>();

        return services;
    }
}