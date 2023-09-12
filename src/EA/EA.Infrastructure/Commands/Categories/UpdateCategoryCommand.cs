using EA.Domain.DTOs;
using EAnalytics.Common.Commands;

namespace EA.Infrastructure.Commands.Categories
{
    public class UpdateCategoryCommand : BaseCommand
    {
        public long CategoryId { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}