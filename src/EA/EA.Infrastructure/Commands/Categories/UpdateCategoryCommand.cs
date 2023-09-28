using EAnalytics.Common.Commands;
using EAnalytics.Common.Dtos;

namespace EA.Infrastructure.Commands.Categories
{
    public class UpdateCategoryCommand : BaseCommand
    {
        public long CategoryId { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}