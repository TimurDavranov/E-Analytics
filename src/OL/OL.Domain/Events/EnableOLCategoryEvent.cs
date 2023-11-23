using EAnalytics.Common.Dtos;
using EAnalytics.Common.Events;

namespace OL.Domain;

public class EnableOLCategoryEvent : BaseEvent
{
    public EnableOLCategoryEvent() : base(nameof(EnableOLCategoryEvent))
    {
    }

    public bool Enable { get; set; }
}

public class UpdateOLCategoryEvent : BaseEvent
{
    public UpdateOLCategoryEvent() : base(nameof(UpdateOLCategoryEvent))
    {
    }

    public long SystemId { get; init; }
    public long? ParentId { get; init; }
    public IReadOnlyList<TranslationDto> Translations { get; init; }
}
