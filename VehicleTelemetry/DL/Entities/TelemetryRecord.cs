using System;

namespace VehicleTelemetry.DL.Entities;

public class TelemetryRecord
{
    public int Id { get; set; }
    public Guid DeviceId { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public int EngineRPM { get; set; }
    public decimal FuelLevelPercentage { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}