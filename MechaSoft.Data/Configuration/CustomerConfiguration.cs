using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MechaSoft.Data.Configuration;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer");

        // Primary Key
        builder.HasKey(c => c.Id);

        // Name Value Object Configuration
        builder.OwnsOne(c => c.Name, name =>
        {
            name.Property(n => n.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(100)
                .IsRequired();

            name.Property(n => n.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(100)
                .IsRequired();
        });

        // Basic Properties
        builder.Property(c => c.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Phone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.Nif)
            .HasMaxLength(9)
            .IsRequired(false);

        builder.Property(c => c.CitizenCard)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(c => c.Notes)
            .HasMaxLength(1000)
            .IsRequired(false);

        // Enum Configuration
        builder.Property(c => c.Type)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Address Value Object Configuration
        builder.OwnsOne(c => c.Address, address =>
        {
            address.Property(a => a.Street)
                .HasColumnName("Street")
                .HasMaxLength(200)
                .IsRequired();

            address.Property(a => a.Number)
                .HasColumnName("Number")
                .HasMaxLength(20)
                .IsRequired();

            address.Property(a => a.Parish)
                .HasColumnName("Parish")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(a => a.Municipality)
                .HasColumnName("Municipality")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(a => a.District)
                .HasColumnName("District")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(a => a.PostalCode)
                .HasColumnName("PostalCode")
                .HasMaxLength(8) // XXXX-XXX format
                .IsRequired();

            address.Property(a => a.Complement)
                .HasColumnName("Complement")
                .HasMaxLength(100)
                .IsRequired(false);

            // Ignore computed property
            address.Ignore(a => a.FullAddress);
        });

        // Navigation Properties
        builder.HasMany(c => c.Vehicles)
            .WithOne(v => v.Customer)
            .HasForeignKey(v => v.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.ServiceOrders)
            .WithOne(so => so.Customer)
            .HasForeignKey(so => so.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for better performance
        builder.HasIndex(c => c.Email)
            .IsUnique();

        builder.HasIndex(c => c.Nif)
            .IsUnique()
            .HasFilter("[Nif] IS NOT NULL");

        builder.HasIndex(c => c.Phone);

        // Composite index for address search
        //builder.HasIndex("Municipality", "District")
        //    .HasDatabaseName("IX_Customer_Location");

        // Auditable Entity Configuration
        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired(false);

        builder.Property(c => c.CreatedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(c => c.UpdatedBy)
            .HasMaxLength(100)
            .IsRequired(false);
    }
}
