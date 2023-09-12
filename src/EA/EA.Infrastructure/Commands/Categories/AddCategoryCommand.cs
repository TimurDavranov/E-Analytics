using EA.Domain.DTOs;
using EA.Domain.Primitives.Base;

namespace EA.Infrastructure.Commands.Categories
{
    public class AddCategoryCommand : BaseCommand
    {
        public List<TranslationDto> Translations { get; set; }
    }
}