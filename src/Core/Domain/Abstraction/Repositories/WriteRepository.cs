using Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Domain.Abstraction.Repositories
{
    public interface IWriteRepository<T> where T : class
    {
        abstract Task<T> CreateAsync(T model);
        abstract Task Update(T model);
        abstract Task Delete(T model);
        abstract Task Delete(long Id);
        abstract Task Delete(Guid Id);
    }

    public class WriteRepository<T> : IWriteRepository<T> where T : class
    {
        private readonly IApplicationDbContext _context;
        protected WriteRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T model)
        {
            var entry = await _context.Set<T>().AddAsync(model).ConfigureAwait(false);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public Task Delete(T model)
        {
            _context.Set<T>().Remove(model);
            return _context.SaveChangesAsync();
        }

        public async Task Delete(long Id)
        {
            var dbSet = _context.Set<T>() as DbSet<Base>;
            if (dbSet is null) throw new ArgumentNullException(nameof(dbSet), $"Table {nameof(T)} does not exist!");

            var entity = await dbSet!.FirstOrDefaultAsync(s => s.Id == Id);
            if (entity is null) throw new ArgumentNullException(nameof(T), $"Entity with Id: {Id} not found!");

            dbSet.Remove(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(Guid Id)
        {
            var dbSet = _context.Set<T>() as DbSet<BaseGuid>;
            if (dbSet is null) throw new ArgumentNullException(nameof(dbSet), $"Table {nameof(T)} does not exist!");

            var entity = await dbSet!.FirstOrDefaultAsync(s => s.Id == Id);
            if (entity is null) throw new ArgumentNullException(nameof(T), $"Entity with Id: {Id} not found!");

            dbSet.Remove(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }


        public Task Update(T model)
        {
            return Task.FromResult(_context.Set<T>().Update(model));
        }


        public void BeginTransaction() => _context.BeginTransaction();

        public Task BeginTransactionAsync(CancellationToken cancellationToken = default) => _context.BeginTransactionAsync(cancellationToken);

        public void RollbackTransaction() => _context.RollbackTransaction();

        public Task RollbackTransactionAsync(CancellationToken cancellationToken = default) => _context.RollbackTransactionAsync(cancellationToken);

        public void CommitTransaction() => _context.CommitTransaction();

        public Task CommitTransactionAsync(CancellationToken cancellationToken = default) => _context.CommitTransactionAsync(cancellationToken);
    }
}