using Microsoft.EntityFrameworkCore;
using Storefront.OrderAndShippingService.Entities;

namespace Storefront.OrderAndShippingService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShippingInfo> ShippingInfos { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<ShippingCharge> ShippingCharges { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public override int SaveChanges()
        {
            SetAuditFields();
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderNumber)
                .IsUnique();

            modelBuilder.Entity<Order>()
                .OwnsOne(o => o.ShippingAddress, sa =>
                {
                    sa.Property(p => p.City);
                    sa.HasIndex(p => p.City);
                });

            modelBuilder.Entity<Order>()
                .OwnsOne(o => o.ShippingAddress);

            modelBuilder.Entity<Order>()
                .OwnsOne(o => o.BillingAddress);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey(i => i.OrderId);

            modelBuilder.Entity<ShippingInfo>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<ShippingInfo>()
                .HasIndex(s => s.OrderId)
                .IsUnique();
        }

        public override async Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            SetAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetAuditFields()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is EntityBase && (e.State == EntityState.Added || e.State == EntityState.Modified))
                .ToList();

            foreach (var entry in entries)
            {
                var entity = (EntityBase)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.ModifiedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.ModifiedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
