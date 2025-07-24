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
            CustomerId = request.CustomerId,
            VehicleId = request.VehicleId,
            Description = request.Description,
            Priority = request.Priority,
            Status = ServiceOrderStatus.Pending,
            MechanicId = request.MechanicId,
            EstimatedDelivery = request.EstimatedDelivery,
            CreatedAt = DateTime.UtcNow
        };

        await _dbSet.AddAsync(serviceOrder);
        await _context.SaveChangesAsync();

        return serviceOrder;
    }
}