using System.Text.Json.Serialization;
using EAnalytics.Common.Commands;
using EAnalytics.Common.Dtos;

namespace OL.Infrastructure.Commands.Categories
{
    public class AddCategoryCommand : BaseCommand
    {
        [JsonInclude]
        public List<TranslationDto> Translations { get; set; }
    }
}