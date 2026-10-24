using Domain.Class;
using DB.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction> AddAsync(Transaction transaction)
    {
        await _context.Set<Transaction>().AddAsync(transaction);
        await _context.SaveChangesAsync();

        return transaction;
    }

    public async Task<Transaction?> GetByIdAsync(int id)
    {
        return await _context.Set<Transaction>()
            .Include(t => t.Customer)
            .Include(t => t.Service)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _context.Set<Transaction>()
            .Include(t => t.Customer)
            .Include(t => t.Service)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Set<Transaction>()
            .Include(t => t.Service)
            .Where(t => t.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetByServiceIdAsync(int serviceId)
    {
        return await _context.Set<Transaction>()
            .Where(t => t.ServiceId == serviceId)
            .ToListAsync();
    }

    public async Task<Transaction> UpdateAsync(Transaction transaction)
    {
        // Same guard as in ServiceRepository: never attach a second instance that
        // shares a key with an entity the context already tracks.
        Transaction? tracked = _context.Set<Transaction>().Local
            .FirstOrDefault(t => t.Id == transaction.Id);

        if (tracked is not null && !ReferenceEquals(tracked, transaction))
        {
            _context.Entry(tracked).CurrentValues.SetValues(transaction);
            transaction = tracked;
        }
        else if (tracked is null)
        {
            _context.Set<Transaction>().Update(transaction);
        }

        await _context.SaveChangesAsync();

        return transaction;
    }
}
