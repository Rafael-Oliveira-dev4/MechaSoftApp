using MechaSoft.Data.Context;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechaSoft.Data.Repositories;

public class InspectionRepository : Repository<Inspection>, IInspectionRepository
{
    public InspectionRepository(ApplicationDbContext context) : base(context) { }

    public override async Task<Inspection> SaveAsync(Inspection inspection)
    {
        return await base.SaveAsync(inspection);
    }

    public override async Task<Inspection> UpdateAsync(Inspection inspection)
    {
        return await base.UpdateAsync(inspection);
    }

    public async Task<IEnumerable<Inspection>> GetByVehicleIdAsync(Guid vehicleId)
    {
        return await _dbSet
            .Where(i => i.VehicleId == vehicleId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Inspection>> GetByServiceOrderIdAsync(Guid serviceOrderId)
    {
        return await _dbSet
            .Where(i => i.ServiceOrderId == serviceOrderId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Inspection>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(i => i.InspectionDate >= startDate && i.InspectionDate <= endDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Inspection>> GetByResultAsync(InspectionResult result)
    {
        return await _dbSet
            .Where(i => i.Result == result)
            .ToListAsync();
    }

    public async Task<IEnumerable<Inspection>> GetPendingInspectionsAsync()
    {
        return await _dbSet
            .Where(i => i.Result == InspectionResult.Pending)
            .ToListAsync();
    }
}