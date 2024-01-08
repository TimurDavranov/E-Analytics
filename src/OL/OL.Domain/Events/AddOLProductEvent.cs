using EAnalytics.Common.Dtos;
using EAnalytics.Common.Events;

namespace OL.Domain;

public class AddOLProductEvent : BaseEvent
{
    public AddOLProductEvent() : base(nameof(AddOLProductEvent))
    {
    }

    public long SystemId { get; init; }
    public decimal Price { get; init; }
    public int InstalmentMaxMouth { get; init; }
    public decimal InstalmentMonthlyRepayment { get; init; }
    public IReadOnlyList<TranslationDto> Translations { get; init; }
}
