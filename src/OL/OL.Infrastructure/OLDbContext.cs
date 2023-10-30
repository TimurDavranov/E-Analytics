using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OL.Domain;
using OL.Domain.Entities;
using OL.Domain.Primitives.Entities;

namespace OL.Infrastructure
{
    public class OLDbContext : DbContext, IOLDbContext
    {
        public OLDbContext(DbContextOptions options) : base(options)
        {
        }

        private ChangeTracker ChangeTracker
        {
            get
            {
                return ChangeTracker;
            }
        }

        public DbSet<OLTranslation> Translations { get; set; }
        public DbSet<OLCategory> Categories { get; set; }

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
            modelBuilder.HasDefaultSchema("ol");
        }
    }
}
