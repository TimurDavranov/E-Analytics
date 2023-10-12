using System.Linq.Expressions;
using EA.Domain;
using EA.Domain.Abstraction.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EA.Application.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly IEADbContext _context;
    public Repository(IEADbContext context)
    {
        _context = context;
    }

    public Task<T> GetAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        var _db = _context.Set<T>();

        if (expression is null)
            throw new ArgumentNullException(nameof(expression), "Expression can't be empty!");

        if (include is not null)
        {
            return include(_db).FirstOrDefaultAsync(expression);
        }

        return _db.FirstOrDefaultAsync(expression);
    }

    public IQueryable<T> GetQueryable()
    {
        var _db = _context.Set<T>();
        return _db.AsQueryable();
    }

    public async Task<T> CreateAsync(T model)
    {
        var _db = _context.Set<T>();
        var entry = await _db.AddAsync(model);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task Delete(Expression<Func<T, bool>> expression)
    {
        var _db = _context.Set<T>();
        var entity = await _db.FirstOrDefaultAsync(expression);

        if (entity is null) throw new ArgumentNullException(nameof(T), $"Data with this condition not found!");

        _db.Remove(entity);

        await _context.SaveChangesAsync();
    }

    public Task Delete(T model)
    {
        var _db = _context.Set<T>();
        _db.Remove(model);
        return _context.SaveChangesAsync();
    }

    public Task Update(T model)
    {
        var _db = _context.Set<T>();
        _db.Update(model);
        return _context.SaveChangesAsync();
    }

    public Task Attach(T model)
    {
        var _db = _context.Set<T>();
        _db.Attach(model);
        return _context.SaveChangesAsync();
    }


    public void BeginTransaction() => _context.BeginTransaction();

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default) => _context.BeginTransactionAsync(cancellationToken);

    public void RollbackTransaction() => _context.RollbackTransaction();

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default) => _context.RollbackTransactionAsync(cancellationToken);

    public void CommitTransaction() => _context.CommitTransaction();

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default) => _context.CommitTransactionAsync(cancellationToken);
}