using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MechaSoft.Data.Configuration;

internal class InspectionConfiguration : IEntityTypeConfiguration<Inspection>
{
    public void Configure(EntityTypeBuilder<Inspection> builder)
    {
        builder.ToTable("Inspection");

        // Primary Key
        builder.HasKey(i => i.Id);

        // Foreign Keys
        builder.Property(i => i.VehicleId)
            .IsRequired();

        builder.Property(i => i.ServiceOrderId)
            .IsRequired();

        // Enum Properties
        builder.Property(i => i.Type)
            .HasConversion<string>()
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(i => i.Result)
            .HasConversion<string>()
            .HasMaxLength(15)
            .IsRequired();

        // Date Properties
        builder.Property(i => i.InspectionDate)
            .IsRequired();

        builder.Property(i => i.ExpiryDate)
            .IsRequired();

        // Text Properties
        builder.Property(i => i.Observations)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(i => i.CertificateNumber)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(i => i.InspectionCenter)
            .HasMaxLength(200)
            .IsRequired();

        // Numeric Properties
        builder.Property(i => i.VehicleMileage)
            .IsRequired();

        // Cost Money Value Object Configuration
        builder.OwnsOne(i => i.Cost, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("CostAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("CostCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("EUR")
                .IsRequired();
        });

        // Navigation Properties
        builder.HasOne(i => i.Vehicle)
            .WithMany(v => v.Inspections)
            .HasForeignKey(i => i.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.ServiceOrder)
            .WithMany(so => so.Inspections)
            .HasForeignKey(i => i.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for better performance
        builder.HasIndex(i => i.VehicleId);

        builder.HasIndex(i => i.ServiceOrderId);

        builder.HasIndex(i => i.Type);

        builder.HasIndex(i => i.Result);

        builder.HasIndex(i => i.InspectionDate);

        builder.HasIndex(i => i.ExpiryDate);

        builder.HasIndex(i => new { i.VehicleId, i.Type })
            .HasDatabaseName("IX_Inspection_Vehicle_Type");

        builder.HasIndex(i => new { i.VehicleId, i.InspectionDate })
            .HasDatabaseName("IX_Inspection_Vehicle_Date");

        builder.HasIndex(i => new { i.Result, i.ExpiryDate })
            .HasDatabaseName("IX_Inspection_Result_Expiry");

        builder.HasIndex(i => i.CertificateNumber)
            .IsUnique()
            .HasFilter("[CertificateNumber] IS NOT NULL");

        builder.HasIndex(i => i.InspectionCenter);

        // Composite index for inspection history queries
        builder.HasIndex(i => new { i.VehicleId, i.Type, i.InspectionDate })
            .HasDatabaseName("IX_Inspection_Vehicle_Type_Date");

        // Index for expiry alerts
        builder.HasIndex(i => new { i.ExpiryDate, i.Result })
            .HasDatabaseName("IX_Inspection_Expiry_Result")
            .HasFilter("[Result] = 'Approved'");

        // Auditable Entity Configuration
        builder.Property(i => i.CreatedAt)
            .IsRequired();

        builder.Property(i => i.UpdatedAt)
            .IsRequired(false);

        builder.Property(i => i.CreatedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(i => i.UpdatedBy)
            .HasMaxLength(100)
            .IsRequired(false);
    }
}