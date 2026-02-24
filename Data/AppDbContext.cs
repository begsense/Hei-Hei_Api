using Microsoft.EntityFrameworkCore;
using Hei_Hei_Api.Models;

namespace Hei_Hei_Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Animator> Animators { get; set; }
    public DbSet<Hero> Heroes { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderAnimator> OrderAnimators { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderAnimator>()
            .HasOne(oa => oa.Order)
            .WithMany(o => o.OrderAnimators)
            .HasForeignKey(oa => oa.OrderId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<OrderAnimator>()
            .HasOne(oa => oa.Animator)
            .WithMany(a => a.OrderAnimators)
            .HasForeignKey(oa => oa.AnimatorId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<OrderAnimator>()
            .HasOne(oa => oa.Hero)
            .WithMany()
            .HasForeignKey(oa => oa.HeroId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Order)
            .WithMany()
            .HasForeignKey(r => r.OrderId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Hero>()
            .Property(h => h.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Package>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<OrderAnimator>()
            .Property(oa => oa.AssignedAmount)
            .HasPrecision(18, 2);
    }
}
