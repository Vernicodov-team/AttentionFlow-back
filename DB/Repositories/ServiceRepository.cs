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
        await _context.Set<Service>().AddAsync(service);
        await _context.SaveChangesAsync();

        return service;
    }

    public async Task<Service?> GetByIdAsync(int id)
    {
        return await _context.Set<Service>()
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Service?> GetByIdWithCustomerAsync(int id)
    {
        return await _context.Set<Service>()
            .Include(s => s.Customer)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Service>> GetAllAsync()
    {
        return await _context.Set<Service>()
            .ToListAsync();
    }

    public async Task<List<Service>> GetAllWithCustomerAsync()
    {
        return await _context.Set<Service>()
            .Include(s => s.Customer)
            .ToListAsync();
    }

    public async Task<List<Service>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Set<Service>()
            .Where(s => s.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<Service> UpdateAsync(Service service)
    {
        // A scoped context may already track another instance with the same key
        // (for example one loaded via GetByIdAsync). Attaching a second copy in that
        // case throws; copy the values onto the tracked instance instead.
        Service? tracked = _context.Set<Service>().Local
            .FirstOrDefault(s => s.Id == service.Id);

        if (tracked is not null && !ReferenceEquals(tracked, service))
        {
            _context.Entry(tracked).CurrentValues.SetValues(service);
            service = tracked;
        }
        else if (tracked is null)
        {
            _context.Set<Service>().Update(service);
        }

        await _context.SaveChangesAsync();

        return service;
    }
}
