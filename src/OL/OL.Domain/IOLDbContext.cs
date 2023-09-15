using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OL.Domain.Entities;
using OL.Domain.Primitives.Entities;

namespace OL.Domain
{
    public interface IOLDbContext
    {
        DbSet<T> Set<T>() where T : class;
        void BeginTransaction();
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task SaveChangesAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default);
        void SaveChanges(bool acceptAllChangesOnSuccess = true);
        void RollbackTransaction();
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        void CommitTransaction();
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        DbSet<OLTranslation> Translations { get; set; }
        DbSet<OLCategory> Categories { get; set; }

        ChangeTracker ChangeTracker { get; }
    }
}