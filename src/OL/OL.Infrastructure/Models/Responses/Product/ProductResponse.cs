using EAnalytics.Common.Dtos;
using EAnalytics.Common.Queries;

namespace OL.Infrastructure.Models.Responses.Product;

public class ProductResponse : BaseQueryResponse
{
    public Guid Id { get; init; }
    public long SystemId { get; init; }
    public decimal Price { get; init; }
    public int InstalmentMaxMouth { get; init; }
    public decimal InstalmentMonthlyRepayment { get; init; }
    public IList<TranslationDto> Translations { get; init; }
}
