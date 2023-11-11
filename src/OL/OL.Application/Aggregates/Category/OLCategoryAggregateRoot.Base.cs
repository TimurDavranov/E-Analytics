using EAnalytics.Common.Aggregates;
using EAnalytics.Common.Dtos;
using OL.Domain;

namespace OL.Application.Aggregates.Category;

public partial class OLCategoryAggregateRoot : AggregateRootSimple
{
    private bool _active;
    public bool Active { get => _active; set => _active = value; }

    public OLCategoryAggregateRoot()
    {
    }

    public OLCategoryAggregateRoot(Guid id, long SystemId, long? ParentId, IReadOnlyList<TranslationDto> translations)
    {
        RaiseEvent(new AddOLCategoryEvent()
        {
            Id = id,
            SystemId = SystemId,
            ParentId = ParentId,
            Translations = translations
        });
    }

    public void Apply(AddOLCategoryEvent @event)
    {
        _id = @event.Id;
        _active = true;
    }
}
