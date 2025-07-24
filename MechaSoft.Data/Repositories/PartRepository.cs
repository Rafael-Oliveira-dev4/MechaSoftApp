using MechaSoft.Data.Context;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;


namespace MechaSoft.Data.Repositories;

public class PartRepository : Repository<Part>, IPartRepository
{
    public PartRepository(ApplicationDbContext context) : base(context) { }

    public override async Task<Part> SaveAsync(Part part)
    {
        return await base.SaveAsync(part);
    }

    public override async Task<Part> UpdateAsync(Part part)
    {
        return await base.UpdateAsync(part);
    }

    public async Task<Part?> GetByCodeAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be null or empty", nameof(code));

        return await _dbSet
            .FirstOrDefaultAsync(p => p.Code == code);
    }

    public async Task<IEnumerable<Part>> GetByCategoryAsync(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be null or empty", nameof(category));

        return await _dbSet
            .Where(p => p.Category == category)
            .ToListAsync();
    }

    public async Task<IEnumerable<Part>> GetLowStockPartsAsync()
    {
        return await _dbSet
            .Where(p => p.StockQuantity <= p.MinStockLevel)
            .ToListAsync();
    }

    public async Task<IEnumerable<Part>> SearchByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Enumerable.Empty<Part>();

        return await _dbSet
            .Where(p => EF.Functions.Like(p.Name, $"%{name}%"))
            .ToListAsync();
    }
}