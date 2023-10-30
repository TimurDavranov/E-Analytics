using EAnalytics.Common.Aggregates;
using OL.Domain;

namespace OL.Application.Aggregates.Category;

public partial class OLCategoryAggregateRoot : AggregateRootSimple
{
    private bool _active;
    public bool Active { get => _active; set => _active = value; }

    public OLCategoryAggregateRoot()
    {
    }

    public OLCategoryAggregateRoot(Guid id, string NameEn, string NameOz, string NameRu, string NameUz, long SystemId, long? ParentId)
    {
        RaiseEvent(new AddOLCategoryEvent()
        {
            Id = id,
            NameEn = NameEn,
            NameOz = NameOz,
            NameRu = NameRu,
            NameUz = NameUz,
            SystemId = SystemId,
            ParentId = ParentId
        });
    }

    public void Apply(AddOLCategoryEvent @event)
    {
        _id = @event.Id;
        _active = true;
    }
}
