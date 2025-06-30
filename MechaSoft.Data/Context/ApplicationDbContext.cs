using MechaSoft.Data.Configuration;
using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<ServiceOrder> ServiceOrders { get; set; }
        public DbSet<ServiceItem> ServiceItems { get; set; }
        public DbSet<PartItem> PartItems { get; set; }
        public DbSet<Inspection> Inspections { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new PartConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceOrderConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceItemConfiguration());
            modelBuilder.ApplyConfiguration(new PartItemConfiguration());
            modelBuilder.ApplyConfiguration(new InspectionConfiguration());

            modelBuilder.HasDefaultSchema("MechaSoft");

            base.OnModelCreating(modelBuilder);
        }
    }
}
