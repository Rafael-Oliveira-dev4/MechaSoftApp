using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MechaSoft.Data.Configuration;

internal class PartItemConfiguration : IEntityTypeConfiguration<PartItem>
{
    public void Configure(EntityTypeBuilder<PartItem> builder)
    {
        builder.ToTable("PartItem");

        // Composite Primary Key
        builder.HasKey(pi => new { pi.ServiceOrderId, pi.PartId });

        // Basic Properties
        builder.Property(pi => pi.Quantity)
            .IsRequired();

        builder.Property(pi => pi.DiscountPercentage)
            .HasColumnType("decimal(5,2)")
            .IsRequired(false);

        // UnitPrice Money Value Object Configuration
        builder.OwnsOne(pi => pi.UnitPrice, money =>
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
        builder.OwnsOne(pi => pi.TotalPrice, money =>
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

        // Navigation Properties - CORRIGIDO: Parts em vez de PartItems
        builder.HasOne(pi => pi.ServiceOrder)
            .WithMany(so => so.Parts)
            .HasForeignKey(pi => pi.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pi => pi.Part)
            .WithMany()
            .HasForeignKey(pi => pi.PartId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(pi => pi.PartId);
        builder.HasIndex(pi => pi.ServiceOrderId);
    }
}