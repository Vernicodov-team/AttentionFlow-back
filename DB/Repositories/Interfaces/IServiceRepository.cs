using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface IServiceRepository
{
    Task<Service> AddAsync(Service service);

    Task<Service?> GetByIdAsync(Guid id);

    Task<Service?> GetByIdWithSubscriptionsAsync(Guid id);

    Task<Service?> GetByNameAsync(string name);

    Task<Service?> GetByNameWithSubscriptionsAsync(string name);

    Task<List<Service>> GetAllAsync();

    Task<List<Service>> GetAllWithSubscriptionsAsync();

    Task<Service> UpdateAsync(Service service);
}
