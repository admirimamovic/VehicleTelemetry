using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace VehicleTelemetry.IoC;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Vehicle Telemetry API",
                Version = "v1",
                Description = "RESTful API for receiving and retrieving vehicle telemetry data from vehicle sensors. " +
                             "This API provides endpoints for data ingestion and retrieval operations.",
                Contact = new OpenApiContact
                {
                    Name = "Development Team",
                    Email = "admir.imamovic90@gmail.com",
                    Url = new Uri("https://github.com/admirimamovic")
                }
            });
        });

        return services;
    }
}