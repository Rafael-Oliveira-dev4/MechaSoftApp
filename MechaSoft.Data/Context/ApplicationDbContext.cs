using MechaSoft.Data.Configuration;
using MechaSoft.Domain.Interfaces;
using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MechaSoft.Data.Context;

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
        // Aplicar configurações
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

        // Configurar soft delete global para entidades auditáveis
        ConfigureSoftDeleteFilter(modelBuilder);

        // Schema padrão
        modelBuilder.HasDefaultSchema("MechaSoftCS");

        base.OnModelCreating(modelBuilder);
    }

    private static void ConfigureSoftDeleteFilter(ModelBuilder modelBuilder)
    {
        // Aplicar query filter para soft delete em todas as entidades que implementam IAuditable
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IAuditable).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(IAuditable.IsDeleted));
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    // Método para incluir entidades deletadas nas consultas (quando necessário)
    public IQueryable<T> IncludeDeleted<T>() where T : class, IAuditable
    {
        return Set<T>().IgnoreQueryFilters();
    }

    // Método para obter apenas entidades deletadas
    public IQueryable<T> OnlyDeleted<T>() where T : class, IAuditable
    {
        return Set<T>().IgnoreQueryFilters().Where(e => e.IsDeleted);
    }
}