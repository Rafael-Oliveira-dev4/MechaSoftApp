using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MechaSoft.Data.Configuration;

internal class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
{
    public void Configure(EntityTypeBuilder<ServiceOrder> builder)
    {
        builder.ToTable("ServiceOrder");

        // Primary Key
        builder.HasKey(so => so.Id);

        // Basic Properties
        builder.Property(so => so.OrderNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(so => so.Description)
            .HasMaxLength(1000)
            .IsRequired();

        // Foreign Keys
        builder.Property(so => so.CustomerId)
            .IsRequired();

        builder.Property(so => so.VehicleId)
            .IsRequired();

        builder.Property(so => so.MechanicId)
            .IsRequired(false);

        // Enum Properties
        builder.Property(so => so.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(so => so.Priority)
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();

        // EstimatedCost Money Value Object Configuration
        builder.OwnsOne(so => so.EstimatedCost, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("EstimatedCostAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("EstimatedCostCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("EUR")
                .IsRequired();
        });

        // FinalCost Money Value Object Configuration (Optional)
        builder.OwnsOne(so => so.FinalCost, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("FinalCostAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("FinalCostCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("EUR")
                .IsRequired();
        });

        // Date Properties
        builder.Property(so => so.EstimatedDelivery)
            .IsRequired(false);

        builder.Property(so => so.ActualDelivery)
            .IsRequired(false);

        // Other Properties
        builder.Property(so => so.ActualHours)
            .HasColumnType("decimal(5,2)")
            .IsRequired(false);

        builder.Property(so => so.RequiresInspection)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(so => so.InternalNotes)
            .HasMaxLength(2000)
            .IsRequired(false);

        // Navigation Properties
        builder.HasOne(so => so.Customer)
            .WithMany(c => c.ServiceOrders)
            .HasForeignKey(so => so.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(so => so.Vehicle)
            .WithMany(v => v.ServiceOrders)
            .HasForeignKey(so => so.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(so => so.Mechanic)
            .WithMany(e => e.ServiceOrders)
            .HasForeignKey(so => so.MechanicId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(so => so.Services)
            .WithOne(si => si.ServiceOrder)
            .HasForeignKey(si => si.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(so => so.Parts)
            .WithOne(pi => pi.ServiceOrder)
            .HasForeignKey(pi => pi.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(so => so.Inspections)
            .WithOne(i => i.ServiceOrder)
            .HasForeignKey(i => i.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for better performance
        builder.HasIndex(so => so.OrderNumber)
            .IsUnique();

        builder.HasIndex(so => so.CustomerId);

        builder.HasIndex(so => so.VehicleId);

        builder.HasIndex(so => so.MechanicId)
            .HasFilter("[MechanicId] IS NOT NULL");

        builder.HasIndex(so => so.Status);

        builder.HasIndex(so => so.Priority);

        builder.HasIndex(so => new { so.Status, so.Priority })
            .HasDatabaseName("IX_ServiceOrder_Status_Priority");

        builder.HasIndex(so => so.EstimatedDelivery)
            .HasFilter("[EstimatedDelivery] IS NOT NULL");

        builder.HasIndex(so => so.ActualDelivery)
            .HasFilter("[ActualDelivery] IS NOT NULL");

        builder.HasIndex(so => so.RequiresInspection);

        builder.HasIndex(so => so.CreatedAt);

        // Auditable Entity Configuration
        builder.Property(so => so.CreatedAt)
            .IsRequired();

        builder.Property(so => so.UpdatedAt)
            .IsRequired(false);

        builder.Property(so => so.CreatedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(so => so.UpdatedBy)
            .HasMaxLength(100)
            .IsRequired(false);
    }
}