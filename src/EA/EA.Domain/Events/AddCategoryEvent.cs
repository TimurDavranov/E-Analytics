using EA.Domain.DTOs;
using EA.Domain.Primitives.Base;

namespace EA.Domain.Events
{
    public class AddCategoryEvent : BaseEvent
    {
        public AddCategoryEvent() : base(nameof(AddCategoryEvent))
        {
        }

        public List<TranslationDto> Translations { get; set; }
    }
}