using Microsoft.EntityFrameworkCore;
using System;

namespace VehicleTelemetry.DL.Entities.DB;

public class VehicleTelemetryDbContext : DbContext
{
    public VehicleTelemetryDbContext(DbContextOptions<VehicleTelemetryDbContext> options)
        : base(options)
    {
    }

    public DbSet<TelemetryRecord> TelemetryRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TelemetryRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.DeviceId});

            entity.Property(e => e.Timestamp)
                .HasConversion(
                    v => v.ToString("o"),
                    v => DateTimeOffset.Parse(v))
                .HasColumnType("TEXT");
        });
    }
}