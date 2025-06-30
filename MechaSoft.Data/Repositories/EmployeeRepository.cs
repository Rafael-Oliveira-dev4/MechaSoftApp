

using MechaSoft.Data.Context;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Model;
using System.Data.Entity;

namespace MechaSoft.Data.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }
    public override async Task<Employee> SaveAsync(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));
        // Validate employee data
        await ValidateEmployeeAsync(employee);

        await _dbSet.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }
    public async Task<IEnumerable<Employee>> GetByRoleAsync(EmployeeRole role)
    {
        return await _dbSet
            .Where(e => e.Role == role)
            .OrderBy(e => e.Name.FirstName)
            .ThenBy(e => e.Name.LastName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetMechanicsWithSpecialtyAsync(ServiceCategory specialty)
    {
        return await _dbSet
            .Where(e => e.Role == EmployeeRole.Mechanic &&
                        e.IsActive &&
                        e.Specialties.Contains(specialty))
            .OrderBy(e => e.Name.FirstName)
            .ThenBy(e => e.Name.LastName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
    {
        return await _dbSet
            .Where(e => e.IsActive)
            .OrderBy(e => e.Role)
            .ThenBy(e => e.Name.FirstName)
            .ThenBy(e => e.Name.LastName)
            .ToListAsync();
    }

    public override async Task<Employee> UpdateAsync(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        // Validações específicas do Employee antes de atualizar
        await ValidateEmployeeAsync(employee, isUpdate: true);

        _dbSet.Update(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    // Métodos auxiliares específicos para Employee
    public async Task<bool> EmailExistsAsync(string email, Guid? excludeEmployeeId = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        var query = _dbSet.Where(e => e.Email == email);

        if (excludeEmployeeId.HasValue)
            query = query.Where(e => e.Id != excludeEmployeeId.Value);

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesWithInspectionLicenseAsync()
    {
        return await _dbSet
            .Where(e => e.IsActive &&
                       e.CanPerformInspections &&
                       !string.IsNullOrEmpty(e.InspectionLicenseNumber))
            .OrderBy(e => e.Name.FirstName)
            .ThenBy(e => e.Name.LastName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetMechanicsAsync()
    {
        return await _dbSet
            .Where(e => e.Role == EmployeeRole.Mechanic && e.IsActive)
            .OrderBy(e => e.Name.FirstName)
            .ThenBy(e => e.Name.LastName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesBySpecialtyAsync(ServiceCategory specialty)
    {
        return await _dbSet
            .Where(e => e.IsActive && e.Specialties.Contains(specialty))
            .OrderBy(e => e.Role)
            .ThenBy(e => e.Name.FirstName)
            .ThenBy(e => e.Name.LastName)
            .ToListAsync();
    }

    public async Task<Employee?> GetOwnerAsync()
    {
        return await _dbSet
            .FirstOrDefaultAsync(e => e.Role == EmployeeRole.Owner && e.IsActive);
    }

    public async Task<IEnumerable<Employee>> GetManagersAsync()
    {
        return await _dbSet
            .Where(e => e.Role == EmployeeRole.Manager && e.IsActive)
            .OrderBy(e => e.Name.FirstName)
            .ThenBy(e => e.Name.LastName)
            .ToListAsync();
    }

    public async Task<Employee?> GetEmployeeWithServiceOrdersAsync(Guid employeeId)
    {
        return await _dbSet
            .Include(e => e.ServiceOrders)
            .FirstOrDefaultAsync(e => e.Id == employeeId);
    }

    public async Task<(IEnumerable<Employee> Items, int TotalCount)> GetPagedEmployeesAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        EmployeeRole? role = null,
        bool? isActive = null,
        ServiceCategory? specialty = null)
    {
        if (pageNumber < 1)
            throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));
        if (pageSize < 1)
            throw new ArgumentException("Page size must be greater than 0", nameof(pageSize));

        IQueryable<Employee> query = _dbSet;

        // Filtro por termo de busca (nome ou email)
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(e =>
                e.Name.FirstName.Contains(searchTerm) ||
                e.Name.LastName.Contains(searchTerm) ||
                e.Email.Contains(searchTerm));
        }

        // Filtro por role
        if (role.HasValue)
        {
            query = query.Where(e => e.Role == role.Value);
        }

        // Filtro por status ativo
        if (isActive.HasValue)
        {
            query = query.Where(e => e.IsActive == isActive.Value);
        }

        // Filtro por especialidade
        if (specialty.HasValue)
        {
            query = query.Where(e => e.Specialties.Contains(specialty.Value));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(e => e.Role)
            .ThenBy(e => e.Name.FirstName)
            .ThenBy(e => e.Name.LastName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<int> GetActiveEmployeeCountByRoleAsync(EmployeeRole role)
    {
        return await _dbSet
            .CountAsync(e => e.Role == role && e.IsActive);
    }

    public async Task<IEnumerable<Employee>> GetAvailableMechanicsForServiceAsync(ServiceCategory serviceCategory)
    {
        return await _dbSet
            .Where(e => e.Role == EmployeeRole.Mechanic &&
                       e.IsActive &&
                       e.Specialties.Contains(serviceCategory))
            .OrderBy(e => e.ServiceOrders.Count(so => so.Status == ServiceOrderStatus.InProgress)) // Ordenar por menos trabalhos em andamento
            .ThenBy(e => e.Name.FirstName)
            .ToListAsync();
    }

    private async Task ValidateEmployeeAsync(Employee employee, bool isUpdate = false)
    {
        // Validação de email único
        if (!string.IsNullOrWhiteSpace(employee.Email))
        {
            var emailExists = await EmailExistsAsync(employee.Email, isUpdate ? employee.Id : null);
            if (emailExists)
                throw new InvalidOperationException($"An employee with email '{employee.Email}' already exists.");
        }

        // Validação de licença de inspeção
        if (employee.CanPerformInspections && string.IsNullOrWhiteSpace(employee.InspectionLicenseNumber))
        {
            throw new InvalidOperationException("Employees who can perform inspections must have a license number.");
        }

        // Validação de especialidades para mecânicos
        if (employee.Role == EmployeeRole.Mechanic && !employee.Specialties.Any())
        {
            throw new InvalidOperationException("Mechanics must have at least one specialty.");
        }

        // Validação de taxa horária para mecânicos
        if (employee.Role == EmployeeRole.Mechanic && employee.HourlyRate == null)
        {
            throw new InvalidOperationException("Mechanics must have an hourly rate defined.");
        }

        // Validação de Owner único (apenas um dono ativo)
        if (employee.Role == EmployeeRole.Owner && employee.IsActive)
        {
            var existingOwner = await _dbSet
                .FirstOrDefaultAsync(e => e.Role == EmployeeRole.Owner &&
                                        e.IsActive &&
                                        e.Id != employee.Id);

            if (existingOwner != null)
            {
                throw new InvalidOperationException("There can only be one active owner.");
            }
        }
    }
}