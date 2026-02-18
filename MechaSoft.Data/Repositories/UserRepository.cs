using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Model;
using MechaSoft.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<User> _dbSet;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<User>();
    }

    public async Task<User> SaveAsync(User entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(u => u.Customer)
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbSet
            .Include(u => u.Customer)
            .Include(u => u.Employee)
            .ToListAsync();
    }

    public async Task<User> UpdateAsync(User entity)
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

    public async Task<IEnumerable<User>> FindAsync(System.Linq.Expressions.Expression<Func<User, bool>> predicate)
    {
        return await _dbSet
            .Include(u => u.Customer)
            .Include(u => u.Employee)
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<User?> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<User, bool>> predicate)
    {
        return await _dbSet
            .Include(u => u.Customer)
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(predicate);
    }

    public async Task<bool> ExistsAsync(System.Linq.Expressions.Expression<Func<User, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task<int> CountAsync(System.Linq.Expressions.Expression<Func<User, bool>>? predicate = null)
    {
        if (predicate == null)
            return await _dbSet.CountAsync();
        
        return await _dbSet.CountAsync(predicate);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet
            .Include(u => u.Customer)
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Username == username.ToLower());
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(u => u.Customer)
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Email == email.ToLower());
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _dbSet
            .Include(u => u.Customer)
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _dbSet.AnyAsync(u => u.Username == username.ToLower());
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email == email.ToLower());
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return await _dbSet
            .Include(u => u.Customer)
            .Include(u => u.Employee)
            .Where(u => u.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
    {
        return await _dbSet
            .Include(u => u.Customer)
            .Include(u => u.Employee)
            .Where(u => u.Role == role)
            .ToListAsync();
    }
}
