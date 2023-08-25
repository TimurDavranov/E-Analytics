using Domain.Entities.Olcha;
using Domain.Primitives.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Domain.Abstraction;

public interface IApplicationDbContext : IDisposable
{
    void BeginTransaction();
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    void SaveChanges(bool acceptAllChangesOnSuccess);
    void RollbackTransaction();
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    void CommitTransaction();
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    DbSet<OlchaCategory> OlchaCategories { get; set; }
    DbSet<Translation> Translations { get; set; }

    ChangeTracker ChangeTracker { get; }
}
