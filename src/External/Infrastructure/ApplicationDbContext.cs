using Domain.Abstraction;
using Domain.Entities.Olcha;
using Domain.Primitives.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<OlchaCategory> OlchaCategories { get; set; }

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
            base.Database.BeginTransaction();
        }

        public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return base.Database.BeginTransactionAsync(cancellationToken);
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
            base.Database.RollbackTransaction();
        }

        public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            return base.Database.RollbackTransactionAsync(cancellationToken);
        }

        public void CommitTransaction()
        {
            base.Database.CommitTransaction();
        }

        public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            return base.Database.CommitTransactionAsync(cancellationToken);
        }

        private void AddSoftDeleteFilter(ModelBuilder modelBuilder) =>
            ((List<IMutableEntityType>)modelBuilder.Model.GetEntityTypes().Where(x => typeof(ISoftDelete).IsAssignableFrom(x.ClrType))).ForEach(softDeletableTypeBuilder =>
            {
                var parameter = Expression.Parameter(softDeletableTypeBuilder.ClrType, "p");
                softDeletableTypeBuilder.SetQueryFilter(
                    Expression.Lambda(
                        Expression.Equal(
                            Expression.Property(parameter, nameof(ISoftDelete.Deleted)),
                            Expression.Constant(false)),
                        parameter)
                );
            });
    }
}
