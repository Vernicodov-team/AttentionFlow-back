using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer customer);

    Task<Customer?> GetByIdAsync(Guid id);

    Task<Customer?> GetByEmailAsync(string email);

    Task<List<Customer>> GetAllAsync();

    Task<Customer> UpdateAsync(Customer customer);
}
