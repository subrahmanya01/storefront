using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Storefront.CartService.Entities;

namespace Storefront.CartService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasIndex(c => c.UserId).IsUnique();

                entity.HasIndex(c => c.CreatedAt);
                entity.Property(c => c.UserId).IsRequired();
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => ci.Id);

                entity.HasOne(ci => ci.Cart)
                      .WithMany(c => c.Items)
                      .HasForeignKey(ci => ci.CartId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(ci => ci.CartId);

                entity.HasIndex(ci => new { ci.CartId, ci.ProductId, ci.VariantId })
                      .IsUnique();

                entity.Property(ci => ci.ProductId).IsRequired();
                entity.Property(ci => ci.VariantId).IsRequired();
                entity.Property(ci => ci.Quantity).IsRequired();
                entity.Property(ci => ci.Price).IsRequired();
            });

        }

        public override int SaveChanges()
        {
            SetAuditFields();
            return base.SaveChanges();
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
