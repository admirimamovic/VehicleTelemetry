using System;
using System.ComponentModel.DataAnnotations;

namespace VehicleTelemetry.DL.DTOs.Requests;

public class TelemetryRequestDto
{
    [Required(ErrorMessage = "DeviceId is required")]
    public Guid DeviceId { get; set; }

    [Required(ErrorMessage = "Timestamp is required")]
    public DateTimeOffset Timestamp { get; set; }

    [Required(ErrorMessage = "EngineRPM is required")]
    public int EngineRPM { get; set; }

    [Required(ErrorMessage = "FuelLevelPercentage is required")]
    [Range(0, 100, ErrorMessage = "FuelLevelPercentage must be between 0 and 100")]
    public decimal FuelLevelPercentage { get; set; }

    [Required(ErrorMessage = "Latitude is required")]
    public decimal Latitude { get; set; }

    [Required(ErrorMessage = "Longitude is required")]
    public decimal Longitude { get; set; }
}