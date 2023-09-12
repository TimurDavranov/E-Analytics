using EA.Domain.Entities.Olcha;
using EA.Domain.Primitives.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EA.Domain.Abstraction;

public interface IApplicationDbContext : IDisposable
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

    DbSet<OLCategory> OLCategories { get; set; }
    DbSet<Translation> Translations { get; set; }

    ChangeTracker ChangeTracker { get; }
}
