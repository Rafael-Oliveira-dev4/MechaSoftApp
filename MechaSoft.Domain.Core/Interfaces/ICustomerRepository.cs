using MechaSoft.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechaSoft.Domain.Core.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    new Task<Customer> SaveAsync(Customer customer);
    Task<Customer?> GetByPhoneAsync(string phone);
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer?> GetByNifAsync(string nif);
    Task<IEnumerable<Customer>> SearchByNameAsync(string name);
    new Task<Customer> UpdateAsync(Customer customer);
    Task<bool> EmailExistsAsync(string email, Guid? excludeCustomerId = null);
}
