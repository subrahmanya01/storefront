using Microsoft.EntityFrameworkCore;
using Storefront.UserService.Entities;

namespace Storefront.UserService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<WhishList> WhishLists { get; set; }
        public DbSet<WhishListItem> WhishListItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WhishList>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<WhishList>()
                .HasOne<User>() 
                .WithMany()
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<WhishListItem>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<WhishListItem>()
                .HasOne<WhishList>()
                .WithMany()
                .HasForeignKey(a => a.WhishListId);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Id).IsUnique();
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
