using MechaSoft.Domain.Model;

namespace MechaSoft.Domain.Core.Interfaces;

public interface IServiceRepository : IRepository<Service>
{
    new Task<Service> SaveAsync(Service service);
    Task<IEnumerable<Service>> GetByCategoryAsync(ServiceCategory category);
    Task<IEnumerable<Service>> GetActiveServicesAsync();
    Task<IEnumerable<Service>> SearchByNameAsync(string name);
    new Task<Service> UpdateAsync(Service service);
}