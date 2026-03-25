using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VehicleTelemetry.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplicationDependencies(builder.Configuration);
builder.Logging.ConfigureApplicationLogging(builder.Configuration);

var app = builder.Build();

app.ConfigureApplicationPipeline();

app.Run();

public partial class Program { }