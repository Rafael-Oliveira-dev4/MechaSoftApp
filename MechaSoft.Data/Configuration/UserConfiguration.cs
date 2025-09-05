using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MechaSoft.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        // Primary Key
        builder.HasKey(u => u.Id);

        // Properties
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(
                v => v.ToLower(),
                v => v);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255)
            .HasConversion(
                v => v.ToLower(),
                v => v);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Salt)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.EmailConfirmed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(u => u.LastLoginAt)
            .IsRequired(false);

        builder.Property(u => u.RefreshToken)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(u => u.RefreshTokenExpiryTime)
            .IsRequired(false);

        builder.Property(u => u.FailedLoginAttempts)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(u => u.LockedUntil)
            .IsRequired(false);

        // Foreign Keys
        builder.Property(u => u.CustomerId)
            .IsRequired(false);

        builder.Property(u => u.EmployeeId)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(u => u.Username)
            .IsUnique();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasIndex(u => u.RefreshToken)
            .IsUnique()
            .HasFilter("[RefreshToken] IS NOT NULL");

        // Relationships
        builder.HasOne(u => u.Customer)
            .WithMany()
            .HasForeignKey(u => u.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(u => u.Employee)
            .WithMany()
            .HasForeignKey(u => u.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        // Constraints
        builder.HasCheckConstraint("CK_Users_CustomerOrEmployee", 
            "([CustomerId] IS NULL AND [EmployeeId] IS NOT NULL) OR ([CustomerId] IS NOT NULL AND [EmployeeId] IS NULL) OR ([CustomerId] IS NULL AND [EmployeeId] IS NULL)");
    }
}
