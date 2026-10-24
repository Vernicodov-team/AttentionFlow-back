using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface ISubscriptionRepository
{
    Task<Subscription> AddAsync(Subscription subscription);

    Task<Subscription?> GetByIdAsync(Guid id);

    Task<List<Subscription>> GetAllAsync();

    Task<List<Subscription>> GetByCustomerIdAsync(Guid customerId);

    Task<List<Subscription>> GetByCatalogIdAsync(Guid catalogId);

    Task<Subscription> UpdateAsync(Subscription subscription);
}
