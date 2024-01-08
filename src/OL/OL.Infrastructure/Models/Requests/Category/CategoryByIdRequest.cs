using EAnalytics.Common.Queries;

namespace OL.Infrastructure.Models.Requests.Category;

public class CategoryByIdRequest : BaseQuery
{
    public Guid Id { get; init; }
}