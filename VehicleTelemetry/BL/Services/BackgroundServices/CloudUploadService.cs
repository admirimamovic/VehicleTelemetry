using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehicleTelemetry.BL.ServiceInterfaces;

namespace VehicleTelemetry.BL.Services.BackgroundServices;

public class CloudUploadService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CloudUploadService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

    public CloudUploadService(IServiceProvider serviceProvider, ILogger<CloudUploadService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Cloud Upload Service is starting");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await UploadToCloudAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during cloud upload");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("Cloud Upload Service is stopping");
    }

    private async Task UploadToCloudAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var telemetryService = scope.ServiceProvider.GetRequiredService<IVehicleTelemetryService>();

        var latestRecords = await telemetryService.GetLatestRecordsAsync(5);

        if (latestRecords.Count == 0)
        {
            _logger.LogInformation("No telemetry records to upload to cloud");
            return;
        }

        foreach (var record in latestRecords)
        {
            _logger.LogInformation(
                "Sending data to Cloud for Device {DeviceId} - Timestamp: {Timestamp}, RPM: {RPM}, Fuel: {Fuel}%",
                record.DeviceId,
                record.Timestamp,
                record.EngineRPM,
                record.FuelLevelPercentage
            );
        }

        _logger.LogInformation("Successfully simulated upload of {Count} records to cloud", latestRecords.Count);
    }
}