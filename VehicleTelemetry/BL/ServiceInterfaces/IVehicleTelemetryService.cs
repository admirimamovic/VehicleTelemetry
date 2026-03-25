using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTelemetry.DL.DTOs.Requests;
using VehicleTelemetry.DL.DTOs.Responses;

namespace VehicleTelemetry.BL.ServiceInterfaces;

public interface IVehicleTelemetryService
{
    Task<TelemetryResponseDto> CreateTelemetryRecordAsync(TelemetryRequestDto request);
    Task<TelemetryResponseDto?> GetLatestTelemetryAsync(Guid deviceId);
    Task<List<TelemetryResponseDto>> GetLatestRecordsAsync(int count);
    Task<PagedResult<TelemetryResponseDto>> GetAllTelemetryAsync(int pageNumber, int pageSize);
}