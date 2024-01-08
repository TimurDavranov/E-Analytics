namespace EAnalytics.Common.Queries;

public class GetAllRequest : BaseQuery
{
    
}

public class GetAllResponse<T> : BaseQueryResponse
{
    public IReadOnlyList<T> Data { get; init; }
}
