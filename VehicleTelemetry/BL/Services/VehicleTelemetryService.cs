using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTelemetry.BL.ServiceInterfaces;
using VehicleTelemetry.DL.DTOs.Requests;
using VehicleTelemetry.DL.DTOs.Responses;
using VehicleTelemetry.DL.Entities;
using VehicleTelemetry.DL.Entities.DB;

namespace VehicleTelemetry.BL.Services;

public class VehicleTelemetryService : IVehicleTelemetryService
{
    private readonly VehicleTelemetryDbContext _context;
    private readonly ILogger<VehicleTelemetryService> _logger;

    public VehicleTelemetryService(VehicleTelemetryDbContext context, ILogger<VehicleTelemetryService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TelemetryResponseDto> CreateTelemetryRecordAsync(TelemetryRequestDto request)
    {
        var telemetryRecord = new TelemetryRecord
        {
            DeviceId = request.DeviceId,
            Timestamp = request.Timestamp,
            EngineRPM = request.EngineRPM,
            FuelLevelPercentage = request.FuelLevelPercentage,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };

        _context.TelemetryRecords.Add(telemetryRecord);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Telemetry record created for DeviceId: {DeviceId}", request.DeviceId);

        return MapToResponseDto(telemetryRecord);
    }

    public async Task<TelemetryResponseDto?> GetLatestTelemetryAsync(Guid deviceId)
    {
        var telemetryRecord = await _context.TelemetryRecords
            .Where(t => t.DeviceId == deviceId)
            .OrderByDescending(t => t.Timestamp)
            .FirstOrDefaultAsync();

        return telemetryRecord != null ? MapToResponseDto(telemetryRecord) : null;
    }

    public async Task<List<TelemetryResponseDto>> GetLatestRecordsAsync(int count)
    {
        var records = await _context.TelemetryRecords
            .OrderByDescending(t => t.Timestamp)
            .Take(count)
            .ToListAsync();

        return records.Select(MapToResponseDto).ToList();
    }

    public async Task<PagedResult<TelemetryResponseDto>> GetAllTelemetryAsync(int pageNumber, int pageSize)
    {
        pageNumber = (pageNumber < 1) ? 1 : (pageNumber);
        pageSize = pageSize < 1 ? 10 : (pageSize > 100 ? 100 : pageSize);

        _logger.LogInformation(
            "Fetching telemetry records - Page: {PageNumber}, PageSize: {PageSize}",
            pageNumber,
            pageSize);

        var totalCount = await _context.TelemetryRecords.CountAsync();

        var records = await _context.TelemetryRecords
            .OrderByDescending(t => t.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = records.Select(MapToResponseDto).ToList() ?? new List<TelemetryResponseDto>();

        return PagedResult<TelemetryResponseDto>.Create(dtos, totalCount, pageNumber, pageSize);
    }

    private static TelemetryResponseDto MapToResponseDto(TelemetryRecord record)
    {
        return new TelemetryResponseDto
        {
            DeviceId = record.DeviceId,
            Timestamp = record.Timestamp,
            EngineRPM = record.EngineRPM,
            FuelLevelPercentage = record.FuelLevelPercentage,
            Latitude = record.Latitude,
            Longitude = record.Longitude
        };
    }
}