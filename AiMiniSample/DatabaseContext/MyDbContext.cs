using AiMiniSample.Database_Tables;
using Microsoft.EntityFrameworkCore;

namespace AiMiniSample.DatabaseContext;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<StoreItem> StoreItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Configure User-Pet relationship
        modelBuilder.Entity<Pet>()
            .HasOne(p => p.User)
            .WithMany(u => u.Pets)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure StoreItem entity
        modelBuilder.Entity<StoreItem>()
            .Property(s => s.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<StoreItem>()
            .HasIndex(s => s.Sku)
            .IsUnique()
            .HasFilter("Sku IS NOT NULL");
    }
}