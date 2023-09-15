using System.Linq.Expressions;
using EA.Domain;
using EA.Domain.Abstraction.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EA.Application.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly IEADbContext _context;
    protected Repository(IEADbContext context)
    {
        _context = context;
    }

    public Task<T> GetAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        if (expression is null)
            throw new ArgumentNullException(nameof(expression), "Expression can't be empty!");

        if (include is not null)
        {
            return include(_context.Set<T>()).FirstOrDefaultAsync(expression);
        }

        return _context.Set<T>().FirstOrDefaultAsync(expression);
    }

    public IQueryable<T> GetQueryable()
    {
        return _context.Set<T>().AsQueryable();
    }

    public async Task<T> CreateAsync(T model)
    {
        var entry = await _context.Set<T>().AddAsync(model).ConfigureAwait(false);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task Delete(Expression<Func<T, bool>> expression)
    {
        var entity = await _context.Set<T>().FirstOrDefaultAsync(expression);

        if (entity is null) throw new ArgumentNullException(nameof(T), $"Data with this condition not found!");

        _context.Set<T>().Remove(entity);

        await _context.SaveChangesAsync();
    }

    public Task Delete(T model)
    {
        _context.Set<T>().Remove(model);
        return _context.SaveChangesAsync();
    }

    public Task Update(T model)
    {
        _context.Set<T>().Update(model);
        return _context.SaveChangesAsync();
    }


    public void BeginTransaction() => _context.BeginTransaction();

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default) => _context.BeginTransactionAsync(cancellationToken);

    public void RollbackTransaction() => _context.RollbackTransaction();

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default) => _context.RollbackTransactionAsync(cancellationToken);

    public void CommitTransaction() => _context.CommitTransaction();

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default) => _context.CommitTransactionAsync(cancellationToken);
}