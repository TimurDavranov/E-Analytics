using EAnalytics.Common.Dtos;
using EAnalytics.Common.Events;

namespace OL.Domain;

public class AddOLCategoryEvent : BaseEvent
{
    public AddOLCategoryEvent() : base(nameof(AddOLCategoryEvent))
    {
    }

    public long SystemId { get; init; }
    public long? ParentId { get; init; }
    public IReadOnlyList<TranslationDto> Translations { get; init; }
}
