using EAnalytics.Common.Queries;

namespace OL.Infrastructure.Models.Responses.Category;

public class CategoryIdsResponse : BaseQueryResponse
{
    public Guid Id { get; init; }
    public long SystemId { get; init; }
}