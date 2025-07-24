using MechaSoft.Data.Context;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Core.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System.Text;


namespace MechaSoft.Data.Uow;

internal class UnitOfWork(
    ApplicationDbContext context,
    ICreateServiceOrderUseCase createServiceOrderUseCase,
    ICustomerRepository customerRepository,
    IEmployeeRepository employeeRepository,
    IInspectionRepository inspectionRepository,
    IPartRepository partRepository,
    IScheduleInspectionUseCase scheduleInspectionUseCase,
    IServiceOrderRepository serviceOrderRepository,
    IServiceRepository serviceRepository,
    IVehicleRepository vehicleRepository
    // Adicione outros serviços conforme necessário
    // ITokenService tokenService,
    // IDataProtectionService dataProtectionService
    ) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    // Repository Properties (conforme a interface)
    public ICreateServiceOrderUseCase CreateServiceOrderUseCase => createServiceOrderUseCase;
    public ICustomerRepository CustomerRepository => customerRepository;
    public IEmployeeRepository EmployeeRepository => employeeRepository;
    public IInspectionRepository InspectionRepository => inspectionRepository;
    public IPartRepository PartRepository => partRepository;
    public IScheduleInspectionUseCase ScheduleInspectionUseCase => scheduleInspectionUseCase;
    public IServiceOrderRepository ServiceOrderRepository => serviceOrderRepository;
    public IServiceRepository ServiceRepository => serviceRepository;
    public IVehicleRepository VehicleRepository => vehicleRepository;

    // Service Properties (descomente se necessário)
    // public ITokenService TokenService => tokenService;
    // public IDataProtectionService DataProtectionService => dataProtectionService;

    public bool Commit()
    {
        return context.SaveChanges() > 0;
    }

    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }

    public IEnumerable<string> DebugChanges()
    {
        var changes = new StringBuilder();
        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
            {
                changes.AppendLine($"Entity: {entry.Entity.GetType().Name}");
                changes.AppendLine($"State: {entry.State}");

                foreach (var property in entry.OriginalValues.Properties)
                {
                    var originalValue = entry.OriginalValues[property]?.ToString();
                    var currentValue = entry.CurrentValues[property]?.ToString();

                    if (entry.State == EntityState.Added)
                    {
                        changes.AppendLine($"Property: {property.Name} | New Value: {currentValue}");
                    }
                    else if (entry.State == EntityState.Deleted)
                    {
                        changes.AppendLine($"Property: {property.Name} | Original Value: {originalValue}");
                    }
                    else if (entry.State == EntityState.Modified && originalValue != currentValue)
                    {
                        changes.AppendLine($"Property: {property.Name} | Original Value: {originalValue} | Current Value: {currentValue}");
                    }
                }
                changes.AppendLine();
            }
        }
        return changes.ToString().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
    }

    public bool HasChanges()
    {
        return context.ChangeTracker.HasChanges();
    }

    public async Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        return _transaction.GetDbTransaction();
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_transaction != null)
            {
                await context.SaveChangesAsync(cancellationToken);
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _transaction?.Dispose();
            context.Dispose();
        }

        _disposed = true;
    }

    private async Task DisposeTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
}