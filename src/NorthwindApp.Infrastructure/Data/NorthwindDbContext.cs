using Microsoft.EntityFrameworkCore;
using NorthwindApp.Domain.Entities;

namespace NorthwindApp.Infrastructure.Data;

/// <summary>
/// Entity Framework Core DbContext for Northwind database
/// </summary>
public class NorthwindDbContext : DbContext
{
    public NorthwindDbContext(DbContextOptions<NorthwindDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<WebTracker> WebTrackers => Set<WebTracker>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Customer configuration
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");
            entity.HasKey(c => c.CustomerId);
            entity.Property(c => c.CustomerId).HasMaxLength(5).IsRequired();
            entity.Property(c => c.CompanyName).HasMaxLength(40).IsRequired();
            entity.Property(c => c.ContactName).HasMaxLength(30);
            entity.Property(c => c.Country).HasMaxLength(15);
            entity.Property(c => c.Phone).HasMaxLength(24);
            entity.Property(c => c.Fax).HasMaxLength(24);
        });

        // Order configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(o => o.OrderId);
            entity.HasOne(o => o.Customer)
                  .WithMany(c => c.Orders)
                  .HasForeignKey(o => o.CustomerId);
        });

        // WebTracker configuration
        modelBuilder.Entity<WebTracker>(entity =>
        {
            entity.ToTable("WebTracker");
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Id).ValueGeneratedOnAdd();
            entity.Property(w => w.UrlRequest).HasMaxLength(500).IsRequired();
            entity.Property(w => w.SourceIp).HasMaxLength(50).IsRequired();
        });
    }
}
