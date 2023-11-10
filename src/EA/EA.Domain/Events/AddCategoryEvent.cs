using EAnalytics.Common.Dtos;
using EAnalytics.Common.Events;

namespace EA.Domain.Events
{
    public class AddCategoryEvent : BaseEvent
    {
        public AddCategoryEvent() : base(nameof(AddCategoryEvent))
        {
        }

        public List<TranslationDto> Translations { get; set; }
        public Guid Parent { get; set; }
    }
}