using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MechaSoft.Data.Configuration;

internal class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Service");

        // Primary Key
        builder.HasKey(s => s.Id);

        // Basic Properties
        builder.Property(s => s.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(s => s.Description)
            .HasMaxLength(1000)
            .IsRequired();

        // Enum Configuration
        builder.Property(s => s.Category)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        // Decimal Properties
        builder.Property(s => s.EstimatedHours)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        // PricePerHour Money Value Object Configuration
        builder.OwnsOne(s => s.PricePerHour, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("PricePerHourAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("PricePerHourCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("EUR")
                .IsRequired();
        });

        // FixedPrice Money Value Object Configuration (Optional)
        builder.OwnsOne(s => s.FixedPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("FixedPriceAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("FixedPriceCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("EUR")
                .IsRequired();
        });

        // Boolean Properties
        builder.Property(s => s.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(s => s.RequiresInspection)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes for better performance
        builder.HasIndex(s => s.Name)
            .IsUnique();

        builder.HasIndex(s => s.Category);

        builder.HasIndex(s => s.IsActive);

        builder.HasIndex(s => new { s.Category, s.IsActive })
            .HasDatabaseName("IX_Service_Category_IsActive");

        // Auditable Entity Configuration
        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .IsRequired(false);

        builder.Property(s => s.CreatedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(s => s.UpdatedBy)
            .HasMaxLength(100)
            .IsRequired(false);
    }
}