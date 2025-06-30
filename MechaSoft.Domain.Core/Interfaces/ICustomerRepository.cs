using MechaSoft.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechaSoft.Domain.Core.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer> SaveAsync(Customer customer);
    Task<Customer?> GetByPhoneAsync(string phone);
    Task<Customer?> GetByNifAsync(string nif);
    Task<IEnumerable<Customer>> SearchByNameAsync(string name);
    Task<Customer> UpdateAsync(Customer customer);

}
