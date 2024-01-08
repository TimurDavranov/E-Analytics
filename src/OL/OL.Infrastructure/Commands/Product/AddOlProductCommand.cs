using System.Text.Json.Serialization;
using EAnalytics.Common.Commands;
using EAnalytics.Common.Dtos;

namespace OL.Infrastructure.Commands.Product;

public class AddOlProductCommand : BaseCommand
{
    public long SystemId { get; init; }
    public string[] SystemImageUrl { get; init; }
    public decimal Price { get; init; }
    public long SystemCategoryId { get; init; }
    public int InstalmentMaxMouth { get; init; }
    public decimal InstalmentMonthlyRepayment { get; init; }
    [JsonInclude]
    public IList<TranslationDto> Translations { get; init; }
}