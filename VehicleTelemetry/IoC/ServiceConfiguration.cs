using Microsoft.Extensions.DependencyInjection;
using VehicleTelemetry.BL.ServiceInterfaces;
using VehicleTelemetry.BL.Services;

namespace VehicleTelemetry.IoC;

public static class ServiceConfiguration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IVehicleTelemetryService, VehicleTelemetryService>();

        return services;
    }
}