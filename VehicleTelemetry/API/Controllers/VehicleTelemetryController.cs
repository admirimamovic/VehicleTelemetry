using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTelemetry.BL.ServiceInterfaces;
using VehicleTelemetry.DL.DTOs.Requests;
using VehicleTelemetry.DL.DTOs.Responses;

namespace VehicleTelemetry.API.Controllers;

[ApiController]
[Route("api/v1/telemetry")]
public class VehicleTelemetryController : ControllerBase
{
    private readonly IVehicleTelemetryService _telemetryService;
    private readonly ILogger<VehicleTelemetryController> _logger;

    public VehicleTelemetryController(IVehicleTelemetryService telemetryService, ILogger<VehicleTelemetryController> logger)
    {
        _telemetryService = telemetryService;
        _logger = logger;
    }

    /// <summary>
    /// Accepts a single telemetry record from a vehicle sensor
    /// </summary>
    /// <param name="request">Telemetry data</param>
    /// <returns>Created telemetry record</returns>
    [HttpPost]
    [ProducesResponseType(typeof(TelemetryResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TelemetryResponseDto>> CreateTelemetryRecord([FromBody] TelemetryRequestDto request)
    {
        _logger.LogInformation(
            "Received telemetry data for DeviceId: {DeviceId}",
            request.DeviceId);

        var result = await _telemetryService.CreateTelemetryRecordAsync(request);
        return CreatedAtAction(
            nameof(GetLatestTelemetry),
            new { deviceId = result.DeviceId },
            result);

    }

    /// <summary>
    /// Retrieves the most recent telemetry record for a specific device
    /// </summary>
    /// <param name="deviceId">Unique device identifier</param>
    /// <returns>Latest telemetry record</returns>
    [HttpGet("{deviceId}/latest")]
    [ProducesResponseType(typeof(TelemetryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TelemetryResponseDto>> GetLatestTelemetry(Guid deviceId)
    {
        _logger.LogInformation(
        "Retrieving latest telemetry for DeviceId: {DeviceId}",
        deviceId);

        var result = await _telemetryService.GetLatestTelemetryAsync(deviceId);

        if (result == null)
        {
            throw new KeyNotFoundException($"No telemetry data found for DeviceId: {deviceId}");
        }

        return Ok(result);
    }

    /// <summary>
    /// Retrieves all telemetry records with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1, min: 1)</param>
    /// <param name="pageSize">Number of items per page (default: 10, min: 1, max: 100)</param>
    /// <returns>Paginated list of telemetry records</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TelemetryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResult<TelemetryResponseDto>>> GetAllTelemetry(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation(
            "Retrieving all telemetry records - Page: {PageNumber}, PageSize: {PageSize}",
            pageNumber,
            pageSize);

        var result = await _telemetryService.GetAllTelemetryAsync(pageNumber, pageSize);

        return Ok(result);
    }
}