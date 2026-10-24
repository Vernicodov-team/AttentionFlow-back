using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction> AddAsync(Transaction transaction);

    Task<Transaction?> GetByIdAsync(int id);

    Task<List<Transaction>> GetAllAsync();

    Task<List<Transaction>> GetByCustomerIdAsync(Guid customerId);

    Task<List<Transaction>> GetByServiceIdAsync(int serviceId);

    Task<Transaction> UpdateAsync(Transaction transaction);
}
