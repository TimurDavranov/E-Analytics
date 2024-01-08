using EAnalytics.Common.Aggregates;
using EAnalytics.Common.Dtos;
using OL.Domain;

namespace OL.Application.Aggregates.Product;

public class OLProductAggregateRoot : AggregateRootSimple
{
    private bool _active;
    public bool Active { get => _active; set => _active = value; }

    public OLProductAggregateRoot()
    {
    }

    public OLProductAggregateRoot(Guid id, long SystemId, decimal Price, int MaxMonth, decimal MonthlyRepayment, IReadOnlyList<TranslationDto> translations)
    {
        RaiseEvent(new AddOLProductEvent()
        {
            Id = id,
            SystemId = SystemId,
            Translations = translations,
            Price = Price,
            InstalmentMaxMouth = MaxMonth,
            InstalmentMonthlyRepayment = MonthlyRepayment
        });
    }
    
    public void Apply(AddOLProductEvent @event)
    {
        _id = @event.Id;
        _active = true;
    }
    
    public void UpdateCategory(Guid id, decimal Price, int MaxMonth, decimal MonthlyRepayment, IReadOnlyList<TranslationDto> translations)
    {
        RaiseEvent(new UpdateOlProductEvent
        {
            Id = id,
            Translations = translations,
            Price = Price,
            InstalmentMaxMouth = MaxMonth,
            InstalmentMonthlyRepayment = MonthlyRepayment
        });
    }

    public void Apply(UpdateOlProductEvent @event)
    {
        _id = @event.Id;
        _active = true;
    }
}
