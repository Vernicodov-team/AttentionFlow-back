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

    public async Task<Subscription?> GetByIdAsync(int id)
    {
        return await _context.Set<Subscription>()
            .Include(s => s.Customer)
            .Include(s => s.Service)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Subscription>> GetAllAsync()
    {
        return await _context.Set<Subscription>()
            .Include(s => s.Customer)
            .Include(s => s.Service)
            .ToListAsync();
    }

    public async Task<List<Subscription>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Set<Subscription>()
            .Include(s => s.Service)
            .Where(s => s.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<List<Subscription>> GetByCatalogIdAsync(int ServiceId)
    {
        return await _context.Set<Subscription>()
            .Include(s => s.Customer)
            .Where(s => s.ServiceId == ServiceId)
            .ToListAsync();
    }

    public async Task<Subscription> UpdateAsync(Subscription subscription)
    {
        _context.Set<Subscription>().Update(subscription);
        await _context.SaveChangesAsync();

        return subscription;
    }
}
