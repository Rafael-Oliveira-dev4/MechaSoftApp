using MechaSoft.Data.Context;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.Data.Repositories;

public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Vehicle> SaveAsync(Vehicle vehicle)
    {
        if (vehicle == null)
            throw new ArgumentNullException(nameof(vehicle));
        
        await _dbSet.AddAsync(vehicle);
        return vehicle;
    }

    public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
    {
        if (string.IsNullOrWhiteSpace(licensePlate))
            throw new ArgumentException("License plate cannot be null or empty", nameof(licensePlate));
        return await _dbSet.FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);
    }

    public async Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _dbSet
            .Where(v => v.CustomerId == customerId)
            .OrderBy(v => v.Brand)
            .ThenBy(v => v.Model)
            .ToListAsync();
    }

    public async Task<IEnumerable<Vehicle>> GetVehiclesWithExpiredInspectionAsync()
    {
        return await _dbSet
            .Where(v => v.InspectionExpiryDate.HasValue && v.InspectionExpiryDate.Value < DateTime.Now)
            .OrderBy(v => v.InspectionExpiryDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Vehicle>> GetVehiclesWithInspectionDueSoonAsync(int daysThreshold = 30)
    {
        var dueDate = DateTime.Now.AddDays(daysThreshold);
        return await _dbSet
            .Where(v => v.InspectionExpiryDate.HasValue && v.InspectionExpiryDate.Value > DateTime.Now && v.InspectionExpiryDate.Value <= dueDate)
            .OrderBy(v => v.InspectionExpiryDate)
            .ToListAsync();
    }

    public override async Task<Vehicle> UpdateAsync(Vehicle vehicle)
    {
        if (vehicle == null)
            throw new ArgumentNullException(nameof(vehicle));
        
        _dbSet.Update(vehicle);
        return vehicle;
    }
}