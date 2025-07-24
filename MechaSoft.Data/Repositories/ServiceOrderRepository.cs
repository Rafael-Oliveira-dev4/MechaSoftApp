using MechaSoft.Data.Context;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.Data.Repositories;

public class ServiceOrderRepository : Repository<ServiceOrder>, IServiceOrderRepository
{
    public ServiceOrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<ServiceOrder> SaveAsync(ServiceOrder serviceOrder)
    {
        if (serviceOrder == null)
            throw new ArgumentNullException(nameof(serviceOrder));
        // Validações específicas podem ser adicionadas aqui
        await _dbSet.AddAsync(serviceOrder);
        await _context.SaveChangesAsync();
        return serviceOrder;
    }

    public async Task<ServiceOrder?> GetByOrderNumberAsync(string orderNumber)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            throw new ArgumentException("Order number cannot be null or empty", nameof(orderNumber));
        return await _dbSet.FirstOrDefaultAsync(so => so.OrderNumber == orderNumber);
    }

    public async Task<IEnumerable<ServiceOrder>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _dbSet
            .Where(so => so.CustomerId == customerId)
            .OrderByDescending(so => so.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceOrder>> GetByStatusAsync(ServiceOrderStatus status)
    {
        return await _dbSet
            .Where(so => so.Status == status)
            .OrderByDescending(so => so.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceOrder>> GetByMechanicIdAsync(Guid mechanicId)
    {
        return await _dbSet
            .Where(so => so.MechanicId == mechanicId)
            .OrderByDescending(so => so.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceOrder>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(so => so.CreatedAt >= startDate && so.CreatedAt <= endDate)
            .OrderBy(so => so.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceOrder>> GetPendingInspectionsAsync()
    {
        return await _dbSet
            .Where(so => so.Status == ServiceOrderStatus.WaitingInspection || (so.RequiresInspection && so.Status == ServiceOrderStatus.InProgress))
            .OrderBy(so => so.CreatedAt)
            .ToListAsync();
    }

    public override async Task<ServiceOrder> UpdateAsync(ServiceOrder serviceOrder)
    {
        if (serviceOrder == null)
            throw new ArgumentNullException(nameof(serviceOrder));
        // Validações específicas podem ser adicionadas aqui
        _dbSet.Update(serviceOrder);
        await _context.SaveChangesAsync();
        return serviceOrder;
    }
}