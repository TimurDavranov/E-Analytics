using System.Text.Json.Serialization;
using EAnalytics.Common.Commands;
using EAnalytics.Common.Dtos;

namespace EA.Infrastructure.Commands.Categories
{
    public class EditCategoryCommand : BaseCommand
    {
        public long CategoryId { get; set; }
        [JsonInclude]
        public List<TranslationDto> Translations { get; set; }
    }
}