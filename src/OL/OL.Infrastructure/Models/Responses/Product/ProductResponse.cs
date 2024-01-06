using EAnalytics.Common.Queries;

namespace OL.Infrastructure.Models.Responses.Product;

public class ProductResponse : BaseQueryResponse
{
    public Guid Id { get; init; }
    public long SystemId { get; init; }
}
