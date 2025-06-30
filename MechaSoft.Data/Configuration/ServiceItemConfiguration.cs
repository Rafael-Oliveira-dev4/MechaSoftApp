using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MechaSoft.Data.Configuration;

internal class ServiceItemConfiguration : IEntityTypeConfiguration<ServiceItem>
{
    public void Configure(EntityTypeBuilder<ServiceItem> builder)
    {
        builder.ToTable("ServiceItem");

        // Primary Key
        builder.HasKey(si => si.Id);

        // Foreign Keys
        builder.Property(si => si.ServiceOrderId)
            .IsRequired();

        builder.Property(si => si.ServiceId)
            .IsRequired();

        builder.Property(si => si.MechanicId)
            .IsRequired(false);

        // Basic Properties
        builder.Property(si => si.Quantity)
            .IsRequired();

        builder.Property(si => si.EstimatedHours)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(si => si.ActualHours)
            .HasColumnType("decimal(5,2)")
            .IsRequired(false);

        builder.Property(si => si.DiscountPercentage)
            .HasColumnType("decimal(5,2)")
            .IsRequired(false);

        builder.Property(si => si.Notes)
            .HasMaxLength(500)
            .IsRequired(false);

        // Enum Configuration
        builder.Property(si => si.Status)
            .HasConversion<string>()
            .HasMaxLength(15)
            .IsRequired();

        // UnitPrice Money Value Object Configuration
        builder.OwnsOne(si => si.UnitPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("UnitPriceAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("UnitPriceCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("EUR")
                .IsRequired();
        });

        // TotalPrice Money Value Object Configuration
        builder.OwnsOne(si => si.TotalPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("TotalPriceAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("TotalPriceCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("EUR")
                .IsRequired();
        });

        // Date Properties
        builder.Property(si => si.StartedAt)
            .IsRequired(false);

        builder.Property(si => si.CompletedAt)
            .IsRequired(false);

        // Navigation Properties
        builder.HasOne(si => si.ServiceOrder)
            .WithMany(so => so.Services) // ← Correto: ServiceOrder tem Services
            .HasForeignKey(si => si.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(si => si.Service)
            .WithMany()
            .HasForeignKey(si => si.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(si => si.Mechanic)
            .WithMany()
            .HasForeignKey(si => si.MechanicId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes for better performance
        builder.HasIndex(si => si.ServiceOrderId);
        builder.HasIndex(si => si.ServiceId);
        builder.HasIndex(si => si.MechanicId)
            .HasFilter("[MechanicId] IS NOT NULL");
        builder.HasIndex(si => si.Status);
        builder.HasIndex(si => new { si.ServiceOrderId, si.Status })
            .HasDatabaseName("IX_ServiceItem_ServiceOrder_Status");
        builder.HasIndex(si => si.StartedAt)
            .HasFilter("[StartedAt] IS NOT NULL");
        builder.HasIndex(si => si.CompletedAt)
            .HasFilter("[CompletedAt] IS NOT NULL");
        builder.HasIndex(si => new { si.MechanicId, si.Status })
            .HasDatabaseName("IX_ServiceItem_Mechanic_Status")
            .HasFilter("[MechanicId] IS NOT NULL");

        // Auditable Entity Configuration
        builder.Property(si => si.CreatedAt)
            .IsRequired();

        builder.Property(si => si.UpdatedAt)
            .IsRequired(false);

        builder.Property(si => si.CreatedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(si => si.UpdatedBy)
            .HasMaxLength(100)
            .IsRequired(false);
    }
}
