using EA.Domain.Entities;
using EA.Domain.Primitives.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EA.Domain;

public interface IEADbContext : IDisposable
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

    DbSet<EATranslation> Translations { get; set; }
    DbSet<Category> Categories { get; set; }

    ChangeTracker ChangeTracker { get; }
}
