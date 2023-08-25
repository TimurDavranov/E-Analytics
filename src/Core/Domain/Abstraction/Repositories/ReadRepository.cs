using Domain.Entities.Olcha;
using Microsoft.EntityFrameworkCore.Query;

namespace Domain.Abstraction.Repositories
{
    public interface IReadRepository<T> where T : class
    {
        abstract Task<T?> GetByIdAsync(long id, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        abstract IQueryable<T> GetQueryable();
        bool HasChanges();
    }

    public abstract class ReadRepository<T> : IReadRepository<T> where T : class
    {
        private readonly IApplicationDbContext _context;
        protected ReadRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public abstract Task<T?> GetByIdAsync(long id, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        public abstract IQueryable<T> GetQueryable();
        public bool HasChanges() =>
            _context.ChangeTracker.HasChanges();

    }
}
