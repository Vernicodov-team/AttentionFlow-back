using Domain.Class;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DB;

public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    
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

        entity.Property(e => e.Name)
            .IsRequired();

        entity.Property(e => e.PaymentTerm)
            .HasConversion<string>();

        entity.Property(e => e.Status)
            .HasConversion<string>();

        entity.HasIndex(e => e.CustomerId);

        entity.HasOne(e => e.Customer)
            .WithMany()
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    });

    modelBuilder.Entity<Transaction>(entity =>
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Type)
            .HasConversion<string>();

        entity.Property(e => e.Status)
            .HasConversion<string>();

        entity.HasIndex(e => e.CustomerId);

        entity.HasOne(e => e.Customer)
            .WithMany()
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Service)
            .WithMany()
            .HasForeignKey(e => e.ServiceId)
            .OnDelete(DeleteBehavior.SetNull);
    });
    
    modelBuilder.Entity<User>(entity =>
    {
        entity.HasKey(e => e.Id);
        entity.HasAlternateKey(e => e.Username);
        entity.Property(e => e.Role)
            .HasConversion<string>();
    });
    
    modelBuilder.Entity<ParameterSet>(entity =>
    {
        entity.HasKey(e => e.Id);
        entity.HasAlternateKey(e => e.PSetName);

        entity.ToTable(table =>
        {
            table.HasCheckConstraint(
                "PS_WeightNoAutoprolong",
                "\"WeightNoAutoprolong\" >= 0.0 AND \"WeightNoAutoprolong\" <= 1.0");
            table.HasCheckConstraint(
                "PS_WeightLowBalance",
                "\"WeightLowBalance\" >= 0.0 AND \"WeightLowBalance\" <= 1.0");
            table.HasCheckConstraint(
                "PS_WeightExpiringSoon",
                "\"WeightExpiringSoon\" >= 0.0 AND \"WeightExpiringSoon\" <= 1.0");
            table.HasCheckConstraint(
                "PS_WeightProlongWaitStatus",
                "\"WeightProlongWaitStatus\" >= 0.0 AND \"WeightProlongWaitStatus\" <= 1.0");
        });
    });
    }
}