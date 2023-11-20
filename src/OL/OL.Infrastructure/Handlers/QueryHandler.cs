using EAnalytics.Common.Abstractions.Repositories;
using OL.Domain;
using OL.Domain.Entities;
using OL.Infrastructure.Models.Requests.Category;
using OL.Infrastructure.Models.Responses.Category;

namespace OL.Infrastructure.Handlers;

public interface IQueryHandler
{
    Task<CategoryResponse> HandleAsync(CategoryByIdRequest query);
}

public sealed class QueryHandler : IQueryHandler
{
    private readonly IRepository<OLCategory, OLDbContext> _repository;

    public QueryHandler(IRepository<OLCategory, OLDbContext> repository)
    {
        this._repository = repository;
    }

    public Task<CategoryResponse> HandleAsync(CategoryByIdRequest query)
    {

        return null;
    }
}
