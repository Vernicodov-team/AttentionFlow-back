using Domain.Class;
using DB.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly AppDbContext _context;

    public ServiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Service> AddAsync(Service service)
    {
        await _context.AddAsync(service);
        await _context.SaveChangesAsync();

        return service;
    }

    public async Task<Service?> GetByIdAsync(Guid id)
    {
        return await _context.Set<Service>()
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Service?> GetByIdWithSubscriptionsAsync(Guid id)
    {
        return await _context.Set<Service>()
            .Include(s => s.Subscriptions)
            .ThenInclude(sub => sub.Customer)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Service?> GetByNameAsync(string name)
    {
        return await _context.Set<Service>()
            .FirstOrDefaultAsync(s => s.Name == name);
    }

    public async Task<Service?> GetByNameWithSubscriptionsAsync(string name)
    {
        return await _context.Set<Service>()
            .Include(s => s.Subscriptions)
            .ThenInclude(sub => sub.Customer)
            .FirstOrDefaultAsync(s => s.Name == name);
    }

    public async Task<List<Service>> GetAllAsync()
    {
        return await _context.Set<Service>()
            .ToListAsync();
    }

    public async Task<List<Service>> GetAllWithSubscriptionsAsync()
    {
        return await _context.Set<Service>()
            .Include(s => s.Subscriptions)
            .ThenInclude(sub => sub.Customer)
            .ToListAsync();
    }

    public async Task<Service> UpdateAsync(Service service)
    {
        _context.Set<Service>().Update(service);
        await _context.SaveChangesAsync();

        return service;
    }
}
