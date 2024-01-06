using EAnalytics.Common.Dtos;
using EAnalytics.Common.Events;

namespace OL.Domain;

public class AddOLProductEvent : BaseEvent
{
    public AddOLProductEvent() : base(nameof(AddOLProductEvent))
    {
    }

    public long SystemId { get; init; }
    public IReadOnlyList<TranslationDto> Translations { get; init; }
}
