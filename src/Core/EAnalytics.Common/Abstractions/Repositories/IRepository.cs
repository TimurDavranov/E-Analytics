using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EAnalytics.Common.Abstractions.Repositories;

public interface IRepository<T, D> where T : class where D : DbContext
{
    Task<T> GetAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    T Get(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    IQueryable<T> GetQueryable();

    abstract Task<T> CreateAsync(T model);
    abstract void Update(T model);
    abstract Task UpdateAsync(T model);
    abstract void Attach(T model);
    abstract Task Delete(T model);
    abstract Task Delete(Expression<Func<T, bool>> expression);

    abstract void BeginTransaction();

    abstract Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    abstract void RollbackTransaction();

    abstract Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    abstract void CommitTransaction();

    abstract Task CommitTransactionAsync(CancellationToken cancellationToken = default);
}
