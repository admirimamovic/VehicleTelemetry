using System;

namespace VehicleTelemetry.DL.DTOs.Responses;

public class    TelemetryResponseDto
{
    public Guid DeviceId { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public int EngineRPM { get; set; }
    public decimal FuelLevelPercentage { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}