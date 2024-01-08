using EAnalytics.Common.Dtos;
using EAnalytics.Common.Events;

namespace OL.Domain;

public class UpdateOlProductEvent() : BaseEvent(nameof(UpdateOlProductEvent))
{
    public decimal Price { get; init; }
    public int InstalmentMaxMouth { get; init; }
    public decimal InstalmentMonthlyRepayment { get; init; }
    public IReadOnlyList<TranslationDto> Translations { get; init; }
}
