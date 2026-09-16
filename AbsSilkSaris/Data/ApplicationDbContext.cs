using AbsSilkSaris.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AbsSilkSaris.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<NewsletterSubscriber> NewsletterSubscribers => Set<NewsletterSubscriber>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Category>().HasIndex(c => c.Slug).IsUnique();
        builder.Entity<Product>().HasIndex(p => p.Slug).IsUnique();
        builder.Entity<Product>().HasIndex(p => p.Sku).IsUnique();
        builder.Entity<NewsletterSubscriber>().HasIndex(n => n.Email).IsUnique();
        builder.Entity<Order>().HasIndex(o => o.OrderNumber).IsUnique();
    }
}
