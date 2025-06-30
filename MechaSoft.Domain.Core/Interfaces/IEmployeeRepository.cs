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

}