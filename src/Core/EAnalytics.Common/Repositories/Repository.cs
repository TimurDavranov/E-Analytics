using System.Linq.Expressions;
using EAnalytics.Common.Abstractions.Repositories;
using EAnalytics.Common.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EA.Application.Repositories;

public class Repository<T, D> : IRepository<T, D> where T : class where D : DbContext
{
    private readonly DatabaseContextFactory<D> _contextFactory;
    public Repository(DatabaseContextFactory<D> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public Task<T> GetAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        using var context = _contextFactory.CreateContext();

        var _db = context.Set<T>();

        if (expression is null)
            throw new ArgumentNullException(nameof(expression), "Expression can't be empty!");

        var quert = _db.AsQueryable();

        if (include is not null)
        {
            quert = include(_db);
        }

        return quert.FirstOrDefaultAsync(expression);
    }

    public T Get(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        using var context = _contextFactory.CreateContext();

        var _db = context.Set<T>();

        if (expression is null)
            throw new ArgumentNullException(nameof(expression), "Expression can't be empty!");

        var quert = _db.AsQueryable();

        if (include is not null)
        {
            quert = include(_db);
        }

        return quert.FirstOrDefault(expression);
    }

    public IQueryable<T> GetQueryable()
    {
        using var context = _contextFactory.CreateContext();
        var _db = context.Set<T>();
        return _db.AsQueryable();
    }

    public async Task<T> CreateAsync(T model)
    {
        using var context = _contextFactory.CreateContext();
        var _db = context.Set<T>();
        var entry = await _db.AddAsync(model);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task Delete(Expression<Func<T, bool>> expression)
    {
        using var context = _contextFactory.CreateContext();
        var _db = context.Set<T>();
        var entity = await _db.FirstOrDefaultAsync(expression);

        if (entity is null) throw new ArgumentNullException(nameof(T), $"Data with this condition not found!");

        _db.Remove(entity);

        await context.SaveChangesAsync();
    }

    public Task Delete(T model)
    {
        using var context = _contextFactory.CreateContext();
        var _db = context.Set<T>();
        _db.Remove(model);
        return context.SaveChangesAsync();
    }

    public void Update(T model)
    {
        using var context = _contextFactory.CreateContext();
        var _db = context.Set<T>();
        _db.Update(model);
        context.SaveChanges();
    }

    public Task UpdateAsync(T model)
    {
        using var context = _contextFactory.CreateContext();
        var _db = context.Set<T>();
        _db.Update(model);
        return context.SaveChangesAsync();
    }

    public void Attach(T model)
    {
        using var context = _contextFactory.CreateContext();
        var _db = context.Set<T>();
        _db.Attach(model);
        context.SaveChanges();
    }


    public void BeginTransaction()
    {
        using var context = _contextFactory.CreateContext();
        context.Database.BeginTransaction();
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        using var context = _contextFactory.CreateContext();
        return context.Database.BeginTransactionAsync(cancellationToken);
    }

    public void RollbackTransaction()
    {
        using var context = _contextFactory.CreateContext();
        context.Database.RollbackTransaction();
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        using var context = _contextFactory.CreateContext();
        return context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void CommitTransaction()
    {
        using var context = _contextFactory.CreateContext();
        context.Database.CommitTransaction();
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        using var context = _contextFactory.CreateContext();
        return context.Database.CommitTransactionAsync(cancellationToken);
    }
}