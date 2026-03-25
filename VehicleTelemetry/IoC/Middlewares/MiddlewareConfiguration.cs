using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Text.Json;

namespace VehicleTelemetry.IoC.Middlewares;

public static class MiddlewareConfiguration
{
    public static WebApplication ConfigureRequestPipeline(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();
        
        

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Vehicle Telemetry API v1");
                options.RoutePrefix = "swagger";
                options.DocumentTitle = "Vehicle Telemetry API Documentation";
            });
        }
        
        app.UseHttpsRedirection();

        // app.UseAuthentication();
        // app.UseAuthorization();

        app.MapControllers();
        app.MapHealthChecks("/hc", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json; charset=utf-8";
                var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(report));
                await context.Response.Body.WriteAsync(bytes);
            },
            ResultStatusCodes =
                             {
                               [HealthStatus.Healthy] = StatusCodes.Status200OK,
                               [HealthStatus.Degraded] = StatusCodes.Status200OK,
                               [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                             }
        });
        return app;
    }
}