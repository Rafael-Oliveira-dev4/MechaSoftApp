using MechaSoft.Data.Context;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.Data.Repositories;

public class ServiceRepository : Repository<Service>, IServiceRepository
{
    public ServiceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Service> SaveAsync(Service service)
    {
        if (service == null)
            throw new ArgumentNullException(nameof(service));
        
        await _dbSet.AddAsync(service);
        return service;
    }

    public async Task<IEnumerable<Service>> GetByCategoryAsync(ServiceCategory category)
    {
        return await _dbSet
            .Where(s => s.Category == category)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Service>> GetActiveServicesAsync()
    {
        return await _dbSet
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Service>> SearchByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        return await _dbSet
            .Where(s => s.Name.Contains(name))
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public override async Task<Service> UpdateAsync(Service service)
    {
        if (service == null)
            throw new ArgumentNullException(nameof(service));
        
        _dbSet.Update(service);
        return service;
    }
}