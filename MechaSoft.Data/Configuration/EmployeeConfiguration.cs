using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MechaSoft.Data.Configuration;

internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employee");

        // Primary Key
        builder.HasKey(e => e.Id);

        // Name Value Object Configuration
        builder.OwnsOne(e => e.Name, name =>
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
        builder.Property(e => e.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Phone)
            .HasMaxLength(20)
            .IsRequired();

        // Enum Configuration
        builder.Property(e => e.Role)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Money Value Object Configuration
        builder.OwnsOne(e => e.HourlyRate, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("HourlyRateAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("HourlyRateCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("EUR")
                .IsRequired();
        });

        // Boolean Properties
        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CanPerformInspections)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.InspectionLicenseNumber)
            .HasMaxLength(50)
            .IsRequired(false);

        // Specialties Collection Configuration
        builder.Property(e => e.Specialties)
            .HasConversion(
                v => string.Join(',', v.Select(s => s.ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => Enum.Parse<ServiceCategory>(s))
                      .ToList()
            )
            .HasColumnName("Specialties")
            .HasMaxLength(500);


        builder.HasMany(e => e.ServiceOrders)
        .WithOne(so => so.Mechanic) 
        .HasForeignKey(so => so.MechanicId) 
        .OnDelete(DeleteBehavior.SetNull);//Ver isso.


        // Indexes for better performance
        builder.HasIndex(e => e.Email)
            .IsUnique();

        builder.HasIndex(e => e.Role);

        builder.HasIndex(e => e.IsActive);

        builder.HasIndex(e => e.InspectionLicenseNumber)
            .IsUnique()
            .HasFilter("[InspectionLicenseNumber] IS NOT NULL");

        // Auditable Entity Configuration
        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .IsRequired(false);
    }
}
