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

    public OLProductAggregateRoot(Guid id, long SystemId, IReadOnlyList<TranslationDto> translations)
    {
        RaiseEvent(new AddOLProductEvent()
        {
            Id = id,
            SystemId = SystemId,
            Translations = translations
        });
    }
    
    public void Apply(AddOLProductEvent @event)
    {
        _id = @event.Id;
        _active = true;
    }
    
    public void UpdateCategory(Guid id, IReadOnlyList<TranslationDto> translations)
    {
        RaiseEvent(new UpdateOlProductEvent
        {
            Id = id,
            Translations = translations
        });
    }

    public void Apply(UpdateOlProductEvent @event)
    {
        _id = @event.Id;
        _active = true;
    }
}
