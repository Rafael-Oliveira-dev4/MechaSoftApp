using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MechaSoft.Data.Configuration;

internal class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicle");

        // Primary Key
        builder.HasKey(v => v.Id);

        // Foreign Key
        builder.Property(v => v.CustomerId)
            .IsRequired();

        // Basic Properties
        builder.Property(v => v.Brand)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(v => v.Model)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(v => v.Year)
            .IsRequired();

        builder.Property(v => v.LicensePlate)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(v => v.Color)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(v => v.Mileage)
            .IsRequired(false);

        builder.Property(v => v.ChassisNumber)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(v => v.EngineNumber)
            .HasMaxLength(50)
            .IsRequired(false);

        // Enum Configuration
        builder.Property(v => v.FuelType)
            .HasConversion<string>()
            .HasMaxLength(15)
            .IsRequired();

        // Engine Properties
        builder.Property(v => v.EngineDisplacement)
            .IsRequired(false);

        builder.Property(v => v.EnginePower)
            .IsRequired(false);

        // Portuguese Documentation
        builder.Property(v => v.InspectionExpiryDate)
            .IsRequired(false);

        // Navigation Properties
        builder.HasOne(v => v.Customer)
            .WithMany(c => c.Vehicles)
            .HasForeignKey(v => v.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(v => v.ServiceOrders)
            .WithOne(so => so.Vehicle)
            .HasForeignKey(so => so.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(v => v.Inspections)
            .WithOne(i => i.Vehicle)
            .HasForeignKey(i => i.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for better performance
        builder.HasIndex(v => v.LicensePlate)
            .IsUnique();

        builder.HasIndex(v => v.CustomerId);

        builder.HasIndex(v => new { v.Brand, v.Model })
            .HasDatabaseName("IX_Vehicle_Brand_Model");

        builder.HasIndex(v => v.Year);

        builder.HasIndex(v => v.FuelType);

        builder.HasIndex(v => v.InspectionExpiryDate)
            .HasFilter("[InspectionExpiryDate] IS NOT NULL");

        builder.HasIndex(v => v.ChassisNumber)
            .IsUnique()
            .HasFilter("[ChassisNumber] IS NOT NULL");

        // Auditable Entity Configuration
        builder.Property(v => v.CreatedAt)
            .IsRequired();

        builder.Property(v => v.UpdatedAt)
            .IsRequired(false);

        builder.Property(v => v.CreatedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(v => v.UpdatedBy)
            .HasMaxLength(100)
            .IsRequired(false);
    }
}