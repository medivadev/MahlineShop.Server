using MahlineShop.Modules.Ordering.Domain;
using Microsoft.EntityFrameworkCore;

namespace MahlineShop.Modules.Ordering.Data;

internal class OrderingDbContext(DbContextOptions<OrderingDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("ordering");

        modelBuilder.Entity<Order>(b =>
        {
            b.HasKey(o => o.Id);
            b.Property(o => o.TotalAmount).HasPrecision(18, 2);
            b.Property(o => o.OrderNumber).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<OrderItem>(b =>
        {
            b.HasKey(i => i.Id);
            b.Property(i => i.Price).HasPrecision(18, 2);
            b.Property(i => i.ProductName).HasMaxLength(200).IsRequired();
        });
    }
}