using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MechaSoft.Data.Configuration;

internal class PartConfiguration : IEntityTypeConfiguration<Part>
{
    public void Configure(EntityTypeBuilder<Part> builder)
    {
        builder.ToTable("Part");

        // Primary Key
        builder.HasKey(p => p.Id);

        // Basic Properties
        builder.Property(p => p.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(p => p.Category)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Brand)
            .HasMaxLength(100)
            .IsRequired(false);

        // Stock Properties
        builder.Property(p => p.StockQuantity)
            .IsRequired();

        builder.Property(p => p.MinStockLevel)
            .IsRequired();

        // UnitCost Money Value Object Configuration
        builder.OwnsOne(p => p.UnitCost, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("UnitCostAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("UnitCostCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("EUR")
                .IsRequired();
        });

        // SalePrice Money Value Object Configuration
        builder.OwnsOne(p => p.SalePrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("SalePriceAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("SalePriceCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("EUR")
                .IsRequired();
        });

        // Supplier Properties
        builder.Property(p => p.SupplierName)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(p => p.SupplierContact)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(p => p.Location)
            .HasMaxLength(100)
            .IsRequired(false);

        // Boolean Properties
        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Indexes for better performance
        builder.HasIndex(p => p.Code)
            .IsUnique();

        builder.HasIndex(p => p.Name);

        builder.HasIndex(p => p.Category);

        builder.HasIndex(p => p.Brand)
            .HasFilter("[Brand] IS NOT NULL");

        builder.HasIndex(p => p.IsActive);

        builder.HasIndex(p => new { p.Category, p.IsActive })
            .HasDatabaseName("IX_Part_Category_IsActive");

        builder.HasIndex(p => new { p.StockQuantity, p.MinStockLevel })
            .HasDatabaseName("IX_Part_Stock_Levels");

        builder.HasIndex(p => p.SupplierName)
            .HasFilter("[SupplierName] IS NOT NULL");

        // Auditable Entity Configuration
        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired(false);

        builder.Property(p => p.CreatedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(p => p.UpdatedBy)
            .HasMaxLength(100)
            .IsRequired(false);
    }
}