using EA.Domain.DTOs;
using EA.Domain.Primitives.Base;

namespace EA.Infrastructure.Commands.Categories
{
    public class UpdateCategoryCommand : BaseCommand
    {
        public long CategoryId { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}