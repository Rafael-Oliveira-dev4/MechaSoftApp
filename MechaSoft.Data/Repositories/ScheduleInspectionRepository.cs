using MechaSoft.Data.Context;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MechaSoft.Data.Repositories;

public class ScheduleInspectionRepository : IScheduleInspectionUseCase
{
    private readonly DbContext _context;
    private readonly DbSet<Inspection> _dbSet;

    public ScheduleInspectionRepository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Inspection>();
    }

    public async Task<Inspection> ExecuteAsync(ScheduleInspectionRequest request)
    {
        // Exemplo de lógica para agendar uma inspeção
        var inspection = new Inspection
        {
            // Preencha os campos conforme o ScheduleInspectionRequest
            // Exemplo:
            // Date = request.Date,
            // Location = request.Location,
            // etc.
        };

        await _dbSet.AddAsync(inspection);
        return inspection;
    }

    public async Task<Inspection> SaveAsync(Inspection entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task<Inspection?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<Inspection>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<Inspection> UpdateAsync(Inspection entity)
    {
        _dbSet.Update(entity);
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public async Task<IEnumerable<Inspection>> FindAsync(Expression<Func<Inspection, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<Inspection?> FirstOrDefaultAsync(Expression<Func<Inspection, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Inspection, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task<int> CountAsync(Expression<Func<Inspection, bool>>? predicate = null)
    {
        if (predicate == null)
            return await _dbSet.CountAsync();
        return await _dbSet.CountAsync(predicate);
    }
}