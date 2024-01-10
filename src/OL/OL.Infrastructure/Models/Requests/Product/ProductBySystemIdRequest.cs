using EAnalytics.Common.Dtos;
using EAnalytics.Common.Queries;

namespace OL.Infrastructure.Models.Requests.Product;

public class ProductBySystemIdRequest : BaseQuery
{
    public long SystemId { get; init; }
}

public class ProductBySystemIdsRequest : BaseQuery
{
    public long[] SystemIds { get; init; }
}
