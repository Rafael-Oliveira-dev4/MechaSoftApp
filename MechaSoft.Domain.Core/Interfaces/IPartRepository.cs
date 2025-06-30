using MechaSoft.Domain.Model;

namespace MechaSoft.Domain.Core.Interfaces;

public interface IPartRepository : IRepository<Part>
{
    Task<Part> SaveAsync(Part part);
    Task<Part?> GetByCodeAsync(string code);
    Task<IEnumerable<Part>> GetByCategoryAsync(string category);
    Task<IEnumerable<Part>> GetLowStockPartsAsync();
    Task<IEnumerable<Part>> SearchByNameAsync(string name);
    Task<Part> UpdateAsync(Part part);
}
