using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace VehicleTelemetry.Exceptions;

public class ModelStateInvalidException : Exception
{
    public ModelStateDictionary ModelState { get; }

    public ModelStateInvalidException(ModelStateDictionary modelState)
        : base("One or more validation errors occurred.")
    {
        ModelState = modelState ?? throw new ArgumentNullException(nameof(modelState));
    }
}
