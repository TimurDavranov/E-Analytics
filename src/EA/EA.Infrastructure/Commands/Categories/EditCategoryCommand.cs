using System.Text.Json.Serialization;
using EA.Domain.DTOs;
using EAnalytics.Common.Commands;

namespace EA.Infrastructure.Commands.Categories
{
    public class EditCategoryCommand : BaseCommand
    {
        public long CategoryId { get; set; }
        [JsonInclude]
        public List<TranslationDto> Translations { get; set; }
    }
}