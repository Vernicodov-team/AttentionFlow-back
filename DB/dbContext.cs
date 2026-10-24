using Domain.Class;
using Microsoft.EntityFrameworkCore;

namespace DB;

public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(c =>
        {
            c.HasKey(e => e.Id);
        });
    }
}