using EA.Domain.DTOs;
using EA.Domain.Events;
using EA.Domain.Primitives;
using EAnalytics.Common.Aggregates;

namespace EA.Application.Aggregates;

public class CategoryAggregateRoot : AggregateRootSimple
{
    private bool _active;
    public bool Active { get => _active; set => _active = value; }

    public CategoryAggregateRoot()
    {
    }

    public CategoryAggregateRoot(Guid Id, List<TranslationDto> translations)
    {
        RaiseEvent(new AddCategoryEvent()
        {
            Id = Id,
            Translations = translations
        });
    }

    public void Apply(AddCategoryEvent @event)
    {
        _id = @event.Id;
        _active = true;
    }

}