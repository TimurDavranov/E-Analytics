using EAnalytics.Common.Dtos;
using EAnalytics.Common.Queries;

namespace OL.Infrastructure.Models.Requests.Product;

public class ProductBySystemIdRequest : BaseQuery
{
    public long SystemId { get; init; }
}

public class ProductByNameRequest : BaseQuery
{
    public IList<TranslationDto> Translations { get; set; }
}
