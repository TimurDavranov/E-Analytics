using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OL.Domain.Entities;
using OL.Domain.Primitives.Entities;

namespace OL.Domain
{
    public interface IOLDbContext
    {
        DbSet<T> Set<T>() where T : class;
        Task SaveChangesAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default);
        void SaveChanges(bool acceptAllChangesOnSuccess = true);

        DbSet<OLTranslation> Translations { get; set; }
        DbSet<OLCategory> Categories { get; set; }

        ChangeTracker ChangeTracker { get; }
    }
}