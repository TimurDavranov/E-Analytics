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

    public void EnableCategory(Guid id, bool enable)
    {
        RaiseEvent(new EnableOLCategoryEvent()
        {
            Id = id,
            Enable = enable
        });
    }

    public void Apply(EnableOLCategoryEvent @event)
    {
        _id = @event.Id;
        _active = true;
    }

    public void UpdateCategory(Guid id, long systemId, long? parentId, IReadOnlyList<TranslationDto> translations)
    {
        RaiseEvent(new UpdateOLCategoryEvent
        {
            Id = id,
            ParentId = parentId,
            SystemId = systemId,
            Translations = translations
        });
    }

    public void Apply(UpdateOLCategoryEvent @event)
    {
        _id = @event.Id;
        _active = true;
    }
}
