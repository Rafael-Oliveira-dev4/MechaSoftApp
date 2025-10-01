using MechaSoft.Domain.Model;

namespace MechaSoft.Domain.Core.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee> SaveAsync(Employee employee);
    Task<Employee?> GetByEmailAsync(string email);
    Task<IEnumerable<Employee>> GetByRoleAsync(EmployeeRole role);
    Task<IEnumerable<Employee>> GetMechanicsWithSpecialtyAsync(ServiceCategory specialty);
    Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
    Task<Employee> UpdateAsync(Employee employee);

    Task<(IEnumerable<Employee> Items, int TotalCount)> GetPagedEmployeesAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        EmployeeRole? role = null,
        bool? isActive = null,
        ServiceCategory? specialty = null);

}