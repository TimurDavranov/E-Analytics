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

        private ChangeTracker ChangeTracker
        {
            get
            {
                return ChangeTracker;
            }
        }

        public DbSet<EATranslation> Translations { get; set; }
        public DbSet<Category> Categories { get; set; }

        public void BeginTransaction()
        {
            Database.BeginTransaction();
        }

        public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Database.BeginTransactionAsync(cancellationToken);
        }

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
            // AddSoftDeleteFilter(modelBuilder);
        }
        public void RollbackTransaction()
        {
            Database.RollbackTransaction();
        }

        public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Database.RollbackTransactionAsync(cancellationToken);
        }

        public void CommitTransaction()
        {
            Database.CommitTransaction();
        }

        public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Database.CommitTransactionAsync(cancellationToken);
        }

        // private void AddSoftDeleteFilter(ModelBuilder modelBuilder) =>
        //     ((List<IMutableEntityType>)modelBuilder.Model.GetEntityTypes().Where(x => typeof(IBaseEntity).IsAssignableFrom(x.ClrType))).ForEach(softDeletableTypeBuilder =>
        //     {
        //         var parameter = Expression.Parameter(softDeletableTypeBuilder.ClrType, "p");
        //         softDeletableTypeBuilder.SetQueryFilter(
        //             Expression.Lambda(
        //                 Expression.Equal(
        //                     Expression.Property(parameter, nameof(IBaseEntity.IsDeleted)),
        //                     Expression.Constant(false)),
        //                 parameter)
        //         );
        //     });
    }
}
