using MahlineShop.Modules.Catalog.Products;
using Microsoft.EntityFrameworkCore;

namespace MahlineShop.Modules.Catalog.Data;

// internal: Only this module can see this DbContext.
// The API Host doesn't know it exists.
internal class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define specific database rules here (Fluent API)
        modelBuilder.Entity<Product>().HasKey(p => p.Id);
        modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(200).IsRequired();
        modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);

        // Define the Schema so tables don't clash with other modules
        modelBuilder.HasDefaultSchema("catalog");
    }
}
