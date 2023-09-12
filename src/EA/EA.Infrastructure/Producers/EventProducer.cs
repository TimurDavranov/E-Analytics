using EA.Domain.Primitives.Base;

namespace EA.Infrastructure.Producers;

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