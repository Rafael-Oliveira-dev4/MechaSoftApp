using MechaSoft.Domain.Model;

namespace MechaSoft.Domain.Core.Interfaces;

public interface IVehicleRepository : IRepository<Vehicle>
{
    Task<Vehicle> SaveAsync(Vehicle vehicle);
    Task<Vehicle?> GetByLicensePlateAsync(string licensePlate);
    Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<Vehicle>> GetVehiclesWithExpiredInspectionAsync();
    Task<IEnumerable<Vehicle>> GetVehiclesWithInspectionDueSoonAsync(int daysThreshold = 30);
    Task<Vehicle> UpdateAsync(Vehicle vehicle);
}
