using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface ISubscriptionRepository
{
    Task<Subscription> AddAsync(Subscription subscription);

    Task<Subscription?> GetByIdAsync(int id);

    Task<List<Subscription>> GetAllAsync();

    Task<List<Subscription>> GetByCustomerIdAsync(Guid customerId);

    Task<List<Subscription>> GetByCatalogIdAsync(int catalogId);

    Task<Subscription> UpdateAsync(Subscription subscription);
}
