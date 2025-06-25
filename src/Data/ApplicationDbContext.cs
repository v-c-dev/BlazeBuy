using BlazeBuy.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazeBuy.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        public DbSet<Coupon> Coupons => Set<Coupon>();
        public DbSet<CouponProduct> CouponsProducts => Set<CouponProduct>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ShoppingCart>()
                   .HasIndex(c => new { c.UserId, c.ProductId })
                   .IsUnique();

            builder.Entity<Coupon>()
                   .HasIndex(c => c.Code)
                   .IsUnique();

            builder.Entity<CouponProduct>()
                   .HasKey(cp => new { cp.CouponId, cp.ProductId });

            builder.Entity<CouponProduct>()
                   .HasOne(cp => cp.Coupon)
                   .WithMany(c => c.Products)
                   .HasForeignKey(cp => cp.CouponId);

            builder.Entity<CouponProduct>()
                   .HasOne(cp => cp.Product)
                   .WithMany()                   // no back-collection needed on Product
                   .HasForeignKey(cp => cp.ProductId);

            builder.Entity<Order>()
                   .HasOne(o => o.Coupon)
                   .WithMany()
                   .HasForeignKey(o => o.CouponId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Coupon>()
                   .Property(c => c.Value)
                   .HasColumnType("decimal(10,2)");

            builder.Entity<Order>()
                   .Property(o => o.DiscountAmount)
                   .HasColumnType("decimal(10,2)");
        }
    }
}
