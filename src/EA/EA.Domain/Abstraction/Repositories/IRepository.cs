using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace EA.Domain.Abstraction.Repositories;

public interface IRepository<T> where T : class
{
    Task<T> GetAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    IQueryable<T> GetQueryable();

    abstract Task<T> CreateAsync(T model);
    abstract Task Update(T model);
    abstract Task Delete(T model);
    abstract Task Delete(Expression<Func<T, bool>> expression);

    abstract void BeginTransaction();

    abstract Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    abstract void RollbackTransaction();

    abstract Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    abstract void CommitTransaction();

    abstract Task CommitTransactionAsync(CancellationToken cancellationToken = default);
}
