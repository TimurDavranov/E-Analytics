using EA.Domain.Abstraction;
using EA.Domain.Entities.Olcha;
using EA.Domain.Primitives.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using EAnalytics.Common.Entities;

namespace EA.Infrastructure
{
    public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<OLCategory> OLCategories { get; set; }

        public DbSet<Translation> Translations { get; set; }
        private ChangeTracker ChangeTracker
        {
            get
            {
                return ChangeTracker;
            }
        }

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
            AddSoftDeleteFilter(modelBuilder);
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

        private void AddSoftDeleteFilter(ModelBuilder modelBuilder) =>
            ((List<IMutableEntityType>)modelBuilder.Model.GetEntityTypes().Where(x => typeof(IBaseEntity).IsAssignableFrom(x.ClrType))).ForEach(softDeletableTypeBuilder =>
            {
                var parameter = Expression.Parameter(softDeletableTypeBuilder.ClrType, "p");
                softDeletableTypeBuilder.SetQueryFilter(
                    Expression.Lambda(
                        Expression.Equal(
                            Expression.Property(parameter, nameof(IBaseEntity.IsDeleted)),
                            Expression.Constant(false)),
                        parameter)
                );
            });
    }
}
