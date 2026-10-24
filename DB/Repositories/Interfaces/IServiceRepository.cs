using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface IServiceRepository
{
    Task<Service> AddAsync(Service service);

    Task<Service?> GetByIdAsync(int id);

    Task<Service?> GetByIdWithCustomerAsync(int id);

    Task<List<Service>> GetAllAsync();

    Task<List<Service>> GetAllWithCustomerAsync();

    Task<List<Service>> GetByCustomerIdAsync(int customerId);

    Task<Service> UpdateAsync(Service service);
}
