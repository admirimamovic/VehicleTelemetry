using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using VehicleTelemetry.BL.ServiceInterfaces;
using VehicleTelemetry.BL.Services;
using VehicleTelemetry.DL.DTOs.Requests;
using VehicleTelemetry.DL.Entities.DB;

namespace VehicleTelemetryUnitTests;

public class TelemetryServiceTests
{
    private static VehicleTelemetryDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<VehicleTelemetryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new VehicleTelemetryDbContext(options);
    }

    [Fact]
    public async Task CreateTelemetryRecordAsync_ShouldCreateAndReturnRecord()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var logger = new Mock<ILogger<VehicleTelemetryService>>();
        var service = new VehicleTelemetryService(context, logger.Object);

        var request = new TelemetryRequestDto
        {
            DeviceId = Guid.NewGuid(),
            Timestamp = DateTimeOffset.UtcNow,
            EngineRPM = 3000,
            FuelLevelPercentage = 75.5m,
            Latitude = 45.2671m,
            Longitude = 19.8335m
        };

        // Act
        var result = await service.CreateTelemetryRecordAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.DeviceId, result.DeviceId);
        Assert.Equal(request.EngineRPM, result.EngineRPM);
        Assert.Equal(request.FuelLevelPercentage, result.FuelLevelPercentage);

        var savedRecord = await context.TelemetryRecords.FirstOrDefaultAsync();
        Assert.NotNull(savedRecord);
        Assert.Equal(request.DeviceId, savedRecord.DeviceId);
    }

    [Fact]
    public async Task GetLatestTelemetryAsync_ShouldReturnLatestRecord()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var logger = new Mock<ILogger<VehicleTelemetryService>>();
        var service = new VehicleTelemetryService(context, logger.Object);
        var deviceId = Guid.NewGuid();

        var request1 = new TelemetryRequestDto
        {
            DeviceId = deviceId,
            Timestamp = DateTimeOffset.UtcNow.AddMinutes(-10),
            EngineRPM = 2000,
            FuelLevelPercentage = 80m,
            Latitude = 45.0m,
            Longitude = 19.0m
        };

        var request2 = new TelemetryRequestDto
        {
            DeviceId = deviceId,
            Timestamp = DateTimeOffset.UtcNow,
            EngineRPM = 3000,
            FuelLevelPercentage = 75m,
            Latitude = 45.5m,
            Longitude = 19.5m
        };

        await service.CreateTelemetryRecordAsync(request1);
        await service.CreateTelemetryRecordAsync(request2);

        // Act
        var result = await service.GetLatestTelemetryAsync(deviceId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3000, result.EngineRPM);
        Assert.Equal(75m, result.FuelLevelPercentage);
    }

    [Fact]
    public async Task GetLatestTelemetryAsync_ShouldReturnNull_WhenNoRecordsExist()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var logger = new Mock<ILogger<VehicleTelemetryService>>();
        var service = new VehicleTelemetryService(context, logger.Object);
        var deviceId = Guid.NewGuid();

        // Act
        var result = await service.GetLatestTelemetryAsync(deviceId);

        // Assert
        Assert.Null(result);
    }
}