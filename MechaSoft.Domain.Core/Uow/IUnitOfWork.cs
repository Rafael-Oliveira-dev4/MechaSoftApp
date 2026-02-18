using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechaSoft.Domain.Core.Uow;

public interface IUnitOfWork : IDisposable
{
    // Repository Properties
    ICreateServiceOrderUseCase CreateServiceOrderUseCase { get; }
    ICustomerRepository CustomerRepository { get; }
    IEmployeeRepository EmployeeRepository { get; }
    IInspectionRepository InspectionRepository { get; }
    IPartRepository PartRepository { get; }
    IScheduleInspectionUseCase ScheduleInspectionUseCase { get; }
    IServiceOrderRepository ServiceOrderRepository { get; }
    IServiceRepository ServiceRepository { get; }
    IVehicleRepository VehicleRepository { get; }
    

    // Service Properties
    IUserRepository UserRepository { get; }
    ITokenService TokenService { get; }

    // Commit Operations
    bool Commit();
    Task<bool> CommitAsync(CancellationToken cancellationToken = default);

    // Transaction Operations
    Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    // Utility Methods
    bool HasChanges();
    IEnumerable<string> DebugChanges();
}