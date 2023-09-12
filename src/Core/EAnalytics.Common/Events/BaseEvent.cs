namespace EAnalytics.Common.Events
{
    public class BaseEvent
    {
        protected BaseEvent(string type)
        {
            Type = type;
        }

        public Guid Id { get; set; }
        public string Type { get; set; }
        public int Version { get; set; }
    }
}