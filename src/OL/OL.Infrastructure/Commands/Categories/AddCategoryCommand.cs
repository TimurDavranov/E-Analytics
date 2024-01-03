using System.Text.Json.Serialization;
using EAnalytics.Common.Commands;
using EAnalytics.Common.Dtos;

namespace OL.Infrastructure.Commands.Categories
{
    public class AddOlCategoryCommand : BaseCommand
    {
        public long SystemId { get; set; }
        public long? ParentId { get; set; }
        public string SystemImageUrl { get; set; }
        [JsonInclude]
        public IList<TranslationDto> Translations { get; init; }
    }
}