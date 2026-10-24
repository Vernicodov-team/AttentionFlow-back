using Domain.Class;
using DB.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _context;

    public SubscriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Subscription> AddAsync(Subscription subscription)
    {
        await _context.AddAsync(subscription);
        await _context.SaveChangesAsync();

        return subscription;
    }

    public async Task<Subscription?> GetByIdAsync(Guid id)
    {
        return await _context.Set<Subscription>()
            .Include(s => s.Customer)
            .Include(s => s.Catalog)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Subscription>> GetAllAsync()
    {
        return await _context.Set<Subscription>()
            .Include(s => s.Customer)
            .Include(s => s.Catalog)
            .ToListAsync();
    }

    public async Task<List<Subscription>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Set<Subscription>()
            .Include(s => s.Catalog)
            .Where(s => s.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<List<Subscription>> GetByCatalogIdAsync(Guid catalogId)
    {
        return await _context.Set<Subscription>()
            .Include(s => s.Customer)
            .Where(s => s.CatalogId == catalogId)
            .ToListAsync();
    }

    public async Task<Subscription> UpdateAsync(Subscription subscription)
    {
        _context.Set<Subscription>().Update(subscription);
        await _context.SaveChangesAsync();

        return subscription;
    }
}
