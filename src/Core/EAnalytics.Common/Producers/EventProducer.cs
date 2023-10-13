using EAnalytics.Common.Events;
using EAnalytics.Common.Helpers.RabbitAgent;

namespace EAnalytics.Common.Producers;

public interface IEventProducer
{
    void Produce<T>(string exchange, string route, string queue, T @event) where T : BaseEvent;
}

public class EventProducer : IEventProducer
{
    private readonly IRabbitMessageProducer _rabbitProducer;
    public EventProducer(IRabbitMessageProducer rabbitProducer)
    {
        _rabbitProducer = rabbitProducer;
    }

    public void Produce<T>(string exchange, string route, string queue, T @event) where T : BaseEvent
    {
        if (@event is null)
            throw new ArgumentNullException(nameof(@event), "Event is null");

        _rabbitProducer.Publish(exchange, route, queue, @event);
    }
}