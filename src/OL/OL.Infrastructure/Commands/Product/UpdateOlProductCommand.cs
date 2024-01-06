using System.Text.Json.Serialization;
using EAnalytics.Common.Commands;
using EAnalytics.Common.Dtos;

namespace OL.Infrastructure.Commands.Product;

public class UpdateOlProductCommand : BaseCommand
{
    public long SystemId { get; init; }
    public string[] SystemImageUrl { get; init; }
    [JsonInclude]
    public IList<TranslationDto> Translations { get; init; }
}