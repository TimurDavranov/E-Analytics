using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using EA.Domain;
using EA.Domain.Primitives.Entities;
using EA.Domain.Entities;

namespace EA.Infrastructure
{
    public sealed class EADbContext : DbContext, IEADbContext
    {
        public EADbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<EACategoryTranslation> Translations { get; set; }
        public DbSet<Category> Categories { get; set; }

        public Task SaveChangesAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public void SaveChanges(bool acceptAllChangesOnSuccess = true)
        {
            base.SaveChanges(acceptAllChangesOnSuccess);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ea");

            modelBuilder.Entity<CategoryRelation>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Relations)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<CategoryRelation>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.Parents)
                .HasForeignKey(c => c.ParentId);

        }
    }
}
