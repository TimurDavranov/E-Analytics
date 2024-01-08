using EAnalytics.Common.Dtos;
using EAnalytics.Common.Events;

namespace OL.Domain;

public class UpdateOLCategoryEvent() : BaseEvent(nameof(UpdateOLCategoryEvent))
{
    public long SystemId { get; init; }
    public long? ParentId { get; init; }
    public IReadOnlyList<TranslationDto> Translations { get; init; }
}