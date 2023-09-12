using EA.Application.Extentions.RabbitAgent;
using EA.Domain.Primitives.Base;

namespace EA.Infrastructure.Producers;

public interface IEventProducer
{
    Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent;
}

public class EventProducer : IEventProducer
{
    private readonly IRabbitMessageProducer _rabbitProducer;
    public EventProducer(IRabbitMessageProducer rabbitProducer)
    {
        _rabbitProducer = rabbitProducer;
    }

    public Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
    {
        if (@event is null)
            throw new ArgumentNullException(nameof(@event), "Event is null");


    }
}