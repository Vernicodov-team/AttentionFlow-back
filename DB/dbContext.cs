using Domain.Class;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

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

        modelBuilder.Entity<Service>(entity =>
    {
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired();
        entity.HasIndex(e => e.Name).IsUnique();
        
        entity.Property(e => e.BasePrice);
        
        entity.Property(e => e.DefaultPaymentTerm)
            .HasConversion<string>();

        entity.Property(e => e.Category);
        
        entity.HasMany(e => e.Subscriptions)
            .WithOne(e => e.Catalog)
            .HasForeignKey(e => e.CatalogId)
            .OnDelete(DeleteBehavior.Restrict);
    });

    modelBuilder.Entity<Subscription>(entity =>
    {
        entity.Property(e => e.CustomName);
        
        entity.Property(e => e.PaymentTerm)
            .HasConversion<string>();
        
        entity.Property(e => e.Status)
            .HasConversion<string>();

        entity.HasOne(e => e.Customer)
            .WithMany()
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    });
    
    modelBuilder.Entity<User>(entity =>
    {
        entity.HasKey(e => e.Id);
        entity.HasAlternateKey(e => e.Username);
        entity.Property(e => e.Role)
            .HasConversion<string>();
    });
    }
}