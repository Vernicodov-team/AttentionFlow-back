using Domain.Class;
using DB.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .ToListAsync();
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();

        return customer;
    }
}
