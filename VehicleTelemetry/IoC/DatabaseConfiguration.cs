using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using VehicleTelemetry.DL.Entities.DB;

namespace VehicleTelemetry.IoC;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabaseConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        // SQLite Database
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Data Source=telemetry.db";

        services.AddDbContext<VehicleTelemetryDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        return services;
    }

    public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<VehicleTelemetryDbContext>();

        // Create database and apply migrations
        await context.Database.EnsureCreatedAsync();
    }
}