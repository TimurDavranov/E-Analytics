using EA.Domain.DTOs;
using EAnalytics.Common.Commands;

namespace EA.Infrastructure.Commands.Categories
{
    public class AddCategoryCommand : BaseCommand
    {
        public List<TranslationDto> Translations { get; set; }
    }
}