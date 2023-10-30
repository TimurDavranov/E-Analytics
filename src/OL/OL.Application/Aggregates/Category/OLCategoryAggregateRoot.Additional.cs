using OL.Domain;

namespace OL.Application.Aggregates.Category;

public partial class OLCategoryAggregateRoot
{
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
}
