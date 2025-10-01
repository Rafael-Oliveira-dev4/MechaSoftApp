using MechaSoft.Data.Context;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Model;


namespace MechaSoft.Data.Repositories;

public class CreateServiceOrderUseCaseRepository : Repository<ServiceOrder>, ICreateServiceOrderUseCase
{
    public CreateServiceOrderUseCaseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ServiceOrder> ExecuteAsync(CreateServiceOrderRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var serviceOrder = new ServiceOrder
        {
            Id = Guid.NewGuid(),
            OrderNumber = $"SO-{DateTime.UtcNow:yyyyMMdd-HHmmss}",
            CustomerId = request.CustomerId,
            VehicleId = request.VehicleId,
            Description = request.Description,
            Status = ServiceOrderStatus.Pending,
            Priority = request.Priority,
            EstimatedCost = new Money(request.EstimatedCost, "EUR"),
            EstimatedDelivery = request.EstimatedDelivery,
            MechanicId = request.MechanicId,
            RequiresInspection = request.RequiresInspection,
            InternalNotes = request.InternalNotes,
            CreatedAt = DateTime.UtcNow
        };

        await _dbSet.AddAsync(serviceOrder);

        return serviceOrder;
    }
}