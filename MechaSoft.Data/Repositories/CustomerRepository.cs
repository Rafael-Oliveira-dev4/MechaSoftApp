using MechaSoft.Data.Context;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Model;
using Microsoft.EntityFrameworkCore;


namespace MechaSoft.Data.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }
    public override async Task<Customer> SaveAsync(Customer customer)
    {
        if(customer == null)
            throw new ArgumentNullException(nameof(customer));

        //Validate customer data
        await ValidateCustomerAsync(customer);
        await _dbSet.AddAsync(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
    public async Task<Customer?> GetByPhoneAsync(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be null or empty", nameof(phone));
        
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Phone == phone);
    }
    public async Task<Customer?> GetByNifAsync(string nif)
    {
        if (string.IsNullOrWhiteSpace(nif))
            throw new ArgumentException("NIF cannot be null or empty", nameof(nif));
        
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Nif == nif);
    }
    public async Task<IEnumerable<Customer>> SearchByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        
        return await _dbSet
            .Where(c => c.Name.FirstName.Contains(name) || c.Name.LastName.Contains(name))
            .ToListAsync();
    }
    public override async Task<Customer> UpdateAsync(Customer customer)
    {
        if(customer == null)
            throw new ArgumentNullException(nameof(customer));
        //Validate customer data
        await ValidateCustomerAsync(customer, isUpdate: true);

        _dbSet.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
    public async Task<bool> PhoneExistAsync(string phone, Guid? excludeCustomerId = null)
    {
        if(string.IsNullOrWhiteSpace(phone))
            return false;

        var query = _dbSet.Where(c => c.Phone == phone);

        if(excludeCustomerId.HasValue)
            query = query.Where(c => c.Id != excludeCustomerId.Value);

        return await query.AnyAsync();
    }
    public async Task<bool> NifExistAsync(string nif, Guid? excludeCustomerId = null)
    {
        if (string.IsNullOrWhiteSpace(nif))
            return false;

        var query = _dbSet.
            Where(c => c.Nif == nif);

        if(excludeCustomerId.HasValue)
            query = query.
                Where(c => c.Id != excludeCustomerId.Value);

        return await query.AnyAsync();
    }

    public async Task<(IEnumerable<Customer> Itens, int TotalCount)> GetPagedCustomerAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        bool? isActive = null)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than 0.");
        if(pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");

        IQueryable<Customer> query = _dbSet;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => c.Name.FirstName.Contains(searchTerm) || 
                                     c.Name.LastName.Contains(searchTerm) ||
                                     c.Email.Contains(searchTerm) ||
                                     c.Phone.Contains(searchTerm));
        }


        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(c => c.Name.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }
    private async Task ValidateCustomerAsync(Customer customer, bool isUpdate = false)
    {
        /* Validação de telefone único
        if (!string.IsNullOrWhiteSpace(customer.Phone))
        {
            var phoneExists = await PhoneExistsAsync(customer.Phone, isUpdate ? customer.Id : null);
            if (phoneExists)
                throw new InvalidOperationException($"A customer with phone '{customer.Phone}' already exists.");
        }

        // Validação de NIF único
        if (!string.IsNullOrWhiteSpace(customer.Nif))
        {
            var nifExists = await NifExistsAsync(customer.Nif, isUpdate ? customer.Id : null);
            if (nifExists)
                throw new InvalidOperationException($"A customer with NIF '{customer.Nif}' already exists.");
        }*/
    }
}
