using System;
using System.Collections.Generic;
using MechaSoft.WebAPI.TempModels;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.WebAPI.TempContext;

public partial class DvRoMechaSoftContext : DbContext
{
    public DvRoMechaSoftContext()
    {
    }

    public DvRoMechaSoftContext(DbContextOptions<DvRoMechaSoftContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Inspection> Inspections { get; set; }

    public virtual DbSet<Part> Parts { get; set; }

    public virtual DbSet<PartItem> PartItems { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceItem> ServiceItems { get; set; }

    public virtual DbSet<ServiceOrder> ServiceOrders { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=DV_RO_MechaSoft;User Id=sa;Password=CHANGE_ME_PASSWORD;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e => e.Nif, "IX_Customer_Nif")
                .IsUnique()
                .HasFilter("([Nif] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => e.InspectionLicenseNumber, "IX_Employee_InspectionLicenseNumber")
                .IsUnique()
                .HasFilter("([InspectionLicenseNumber] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.HourlyRateCurrency).HasDefaultValue("EUR");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Inspection>(entity =>
        {
            entity.HasIndex(e => e.CertificateNumber, "IX_Inspection_CertificateNumber")
                .IsUnique()
                .HasFilter("([CertificateNumber] IS NOT NULL)");

            entity.HasIndex(e => new { e.ExpiryDate, e.Result }, "IX_Inspection_Expiry_Result").HasFilter("([Result]='Approved')");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CostCurrency).HasDefaultValue("EUR");
        });

        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasIndex(e => e.Brand, "IX_Part_Brand").HasFilter("([Brand] IS NOT NULL)");

            entity.HasIndex(e => e.SupplierName, "IX_Part_SupplierName").HasFilter("([SupplierName] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.SalePriceCurrency).HasDefaultValue("EUR");
            entity.Property(e => e.UnitCostCurrency).HasDefaultValue("EUR");
        });

        modelBuilder.Entity<PartItem>(entity =>
        {
            entity.Property(e => e.TotalPriceCurrency).HasDefaultValue("EUR");
            entity.Property(e => e.UnitPriceCurrency).HasDefaultValue("EUR");

            entity.HasOne(d => d.Part).WithMany(p => p.PartItems).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FixedPriceCurrency).HasDefaultValue("EUR");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PricePerHourCurrency).HasDefaultValue("EUR");
        });

        modelBuilder.Entity<ServiceItem>(entity =>
        {
            entity.HasIndex(e => e.CompletedAt, "IX_ServiceItem_CompletedAt").HasFilter("([CompletedAt] IS NOT NULL)");

            entity.HasIndex(e => e.MechanicId, "IX_ServiceItem_MechanicId").HasFilter("([MechanicId] IS NOT NULL)");

            entity.HasIndex(e => new { e.MechanicId, e.Status }, "IX_ServiceItem_Mechanic_Status").HasFilter("([MechanicId] IS NOT NULL)");

            entity.HasIndex(e => e.StartedAt, "IX_ServiceItem_StartedAt").HasFilter("([StartedAt] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.TotalPriceCurrency).HasDefaultValue("EUR");
            entity.Property(e => e.UnitPriceCurrency).HasDefaultValue("EUR");

            entity.HasOne(d => d.Mechanic).WithMany(p => p.ServiceItems).OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceItems).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ServiceOrder>(entity =>
        {
            entity.HasIndex(e => e.ActualDelivery, "IX_ServiceOrder_ActualDelivery").HasFilter("([ActualDelivery] IS NOT NULL)");

            entity.HasIndex(e => e.EstimatedDelivery, "IX_ServiceOrder_EstimatedDelivery").HasFilter("([EstimatedDelivery] IS NOT NULL)");

            entity.HasIndex(e => e.MechanicId, "IX_ServiceOrder_MechanicId").HasFilter("([MechanicId] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.EstimatedCostCurrency).HasDefaultValue("EUR");
            entity.Property(e => e.FinalCostCurrency).HasDefaultValue("EUR");

            entity.HasOne(d => d.Customer).WithMany(p => p.ServiceOrders).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Mechanic).WithMany(p => p.ServiceOrders).OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Vehicle).WithMany(p => p.ServiceOrders).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.RefreshToken, "IX_Users_RefreshToken")
                .IsUnique()
                .HasFilter("([RefreshToken] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Customer).WithMany(p => p.Users).OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Employee).WithMany(p => p.Users).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasIndex(e => e.ChassisNumber, "IX_Vehicle_ChassisNumber")
                .IsUnique()
                .HasFilter("([ChassisNumber] IS NOT NULL)");

            entity.HasIndex(e => e.InspectionExpiryDate, "IX_Vehicle_InspectionExpiryDate").HasFilter("([InspectionExpiryDate] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
