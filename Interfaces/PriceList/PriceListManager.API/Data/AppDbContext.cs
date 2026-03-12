using Microsoft.EntityFrameworkCore;
using PriceListManager.API.Models;

namespace PriceListManager.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductMovement> ProductMovements => Set<ProductMovement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Login).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Login).IsUnique();
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Role).HasConversion<string>().HasMaxLength(50);
            entity.HasOne(e => e.Creator)
                  .WithMany()
                  .HasForeignKey(e => e.CreatedBy)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Description).HasColumnType("text");
            entity.HasOne(e => e.Creator)
                  .WithMany()
                  .HasForeignKey(e => e.CreatedBy)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Article).HasMaxLength(100);
            entity.HasIndex(e => e.Article);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Creator)
                  .WithMany()
                  .HasForeignKey(e => e.CreatedBy)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Price);
            entity.HasIndex(e => e.CategoryId);
            entity.HasIndex(e => new { e.Name, e.CategoryId }).IsUnique();
        });

        // ProductMovement
        modelBuilder.Entity<ProductMovement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Product)
                  .WithMany(p => p.Movements)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.FromCategory)
                  .WithMany()
                  .HasForeignKey(e => e.FromCategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.ToCategory)
                  .WithMany()
                  .HasForeignKey(e => e.ToCategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.MovedByUser)
                  .WithMany()
                  .HasForeignKey(e => e.MovedBy)
                  .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
