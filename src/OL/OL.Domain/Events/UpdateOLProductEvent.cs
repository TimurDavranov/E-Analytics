using EAnalytics.Common.Dtos;
using EAnalytics.Common.Events;

namespace OL.Domain;

public class UpdateOlProductEvent() : BaseEvent(nameof(UpdateOlProductEvent))
{
    public IReadOnlyList<TranslationDto> Translations { get; init; }
}
