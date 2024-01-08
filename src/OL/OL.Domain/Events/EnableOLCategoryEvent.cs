using EAnalytics.Common.Events;

namespace OL.Domain;

public class EnableOLCategoryEvent : BaseEvent
{
    public EnableOLCategoryEvent() : base(nameof(EnableOLCategoryEvent))
    {
    }

    public bool Enable { get; set; }
}