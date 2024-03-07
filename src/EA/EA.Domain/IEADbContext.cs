using EA.Domain.Entities;
using EA.Domain.Primitives.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EA.Domain;

public interface IEADbContext
{
    DbSet<T> Set<T>() where T : class;
    Task SaveChangesAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default);
    void SaveChanges(bool acceptAllChangesOnSuccess = true);

    DbSet<EACategoryTranslation> Translations { get; set; }
    DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<SystemProduct> SystemProducts { get; set; }

    ChangeTracker ChangeTracker { get; }
}
