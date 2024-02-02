using System.Linq.Expressions;
using EAnalytics.Common.Abstractions.Repositories;
using EAnalytics.Common.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace EAnalytics.Common.Repositories;

public class Repository<T, D>(DatabaseContextFactory<D> contextFactory) : IRepository<T, D>
    where T : class
    where D : DbContext
{
    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        await using var context = contextFactory.CreateContext();
        var db = context.Set<T>();

        if (expression is null)
            throw new ArgumentNullException(nameof(expression), "Expression can't be empty!");

        var query = db.AsQueryable();

        if (include is not null)
        {
            query = include(db);
        }

        return await query.AsNoTracking().Where(expression).FirstOrDefaultAsync();
    }
    
    public async Task<IReadOnlyList<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Expression<Func<T, TResult>>? select = null)
    {
        await using var context = contextFactory.CreateContext();
        var db = context.Set<T>();


        var query = db.AsQueryable();

        if (include is not null)
            query = include(db);

        if (expression is not null)
            query = query.Where(expression);

        if (select is null)
            throw new ArgumentNullException("Select expression is empty!");
        
        return (await query.AsNoTracking().Select(select).ToListAsync()).AsReadOnly();
    }
    
    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        await using var context = contextFactory.CreateContext();
        var db = context.Set<T>();


        var query = db.AsQueryable();

        if (include is not null)
            query = include(db);

        if (expression is not null)
            query = query.Where(expression);

        var result = await query.AsNoTracking().ToListAsync();

        return result.AsReadOnly();
    }

    public T? Get(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        using var context = contextFactory.CreateContext();
        var db = context.Set<T>();

        if (expression is null)
            throw new ArgumentNullException(nameof(expression), "Expression can't be empty!");

        var query = db.AsQueryable();

        if (include is not null)
        {
            query = include(db);
        }

        var result = query.AsNoTracking().FirstOrDefault(expression);
        return result;
    }

    public IQueryable<T> GetQueryable()
    {
        using var context = contextFactory.CreateContext();
        var db = context.Set<T>();
        return db.AsQueryable();
    }

    public Func<D> CreateContext => contextFactory.CreateContext;

    public async Task<T> CreateAsync(T model)
    {
        await using var context = contextFactory.CreateContext();
        var db = context.Set<T>();
        var entry = await db.AddAsync(model);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task Delete(Expression<Func<T, bool>> expression)
    {
        await using var context = contextFactory.CreateContext();
        var db = context.Set<T>();
        var entity = await db.FirstOrDefaultAsync(expression);

        if (entity is null) throw new ArgumentNullException(nameof(T), $"Data with this condition not found!");

        db.Remove(entity);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T model)
    {
        await using var context = contextFactory.CreateContext();
        var db = context.Set<T>();
        db.Remove(model);
        await context.SaveChangesAsync();
    }

    public void Update(T model)
    {
        using var context = contextFactory.CreateContext();
        var db = context.Set<T>();
        db.Update(model);
        context.SaveChanges();
    }

    public async Task UpdateAsync(T model)
    {
        await using var context = contextFactory.CreateContext();
        var db = context.Set<T>();
        db.Update(model);
        await context.SaveChangesAsync();
    }

    public void Attach(T model, EntityState state)
    {
        using var context = contextFactory.CreateContext();
        var db = context.Set<T>();
        var entity = db.Attach(model);
        entity.State = state;
        context.SaveChanges();
    }


    public void BeginTransaction()
    {
        using var context = contextFactory.CreateContext();
        context.Database.BeginTransaction();
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await using var context = contextFactory.CreateContext();
        await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public void RollbackTransaction()
    {
        using var context = contextFactory.CreateContext();
        context.Database.RollbackTransaction();
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await using var context = contextFactory.CreateContext();
        await context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void CommitTransaction()
    {
        using var context = contextFactory.CreateContext();
        context.Database.CommitTransaction();
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await using var context = contextFactory.CreateContext();
        await context.Database.CommitTransactionAsync(cancellationToken);
    }
}