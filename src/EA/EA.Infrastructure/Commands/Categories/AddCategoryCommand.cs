using System.Text.Json.Serialization;
using EAnalytics.Common.Commands;
using EAnalytics.Common.Dtos;

namespace EA.Infrastructure.Commands.Categories
{
    public class AddCategoryCommand : BaseCommand
    {
        [JsonInclude]
        public List<TranslationDto> Translations { get; set; }

        [JsonInclude] 
        public Guid Parent { get; set; }
    }
}