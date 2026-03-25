using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTelemetry.Exceptions;

namespace VehicleTelemetry.IoC.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ModelStateInvalidException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var errors = ex.ModelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .Select(ms => new
                {
                    field = ms.Key,
                    error = ms.Value.Errors.Select(e => e.ErrorMessage)
                });

            var response = new
            {
                errorCode = "VALIDATION_ERROR",
                message = "One or more validation errors occurred.",
                statusCode = StatusCodes.Status400BadRequest,
                details = errors
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        catch (KeyNotFoundException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status404NotFound;

            var response = new
            {
                errorCode = "NOT_FOUND",
                statusCode = StatusCodes.Status404NotFound,
                message = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new
            {
                errorCode = "INTERNAL_SERVER_ERROR",
                message = "An unexpected error occurred. Please try again later."
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}