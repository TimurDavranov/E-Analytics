using EAnalytics.Common.Events;

namespace OL.Domain;

public class AddOLCategoryEvent : BaseEvent
{
    public AddOLCategoryEvent() : base(nameof(AddOLCategoryEvent))
    {
    }

    public long SystemId { get; set; }
    public long? ParentId { get; set; }
    public string NameRu { get; set; }
    public string NameUz { get; set; }
    public string NameOz { get; set; }
    public string NameEn { get; set; }
}
