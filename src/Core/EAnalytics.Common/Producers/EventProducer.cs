
using EAnalytics.Common.Events;

namespace EAnalytics.Common.Producers;

public interface IEventProducer
{
    Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent;
}

public class EventProducer : IEventProducer
{
    public Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
    {
        throw new NotImplementedException();
    }
}