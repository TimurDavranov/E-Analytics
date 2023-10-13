using EA.Application;
using EA.Infrastructure.Stores;
using EAnalytics.Common.Aggregates;
using EAnalytics.Common.Producers;

namespace EA.Infrastructure.Handlers
{
    public interface IEventSourcingHandler<T> where T : AggregateRootSimple
    {
        Task SaveAsync(AggregateRootSimple aggregate);
        Task<T> GetByIdAsync(Guid aggregateId);
        Task RepublishEventsAsync(string exchangeKey, string routeKey, string queueKey);
    }

    public class EventSourcingHandler<T> : IEventSourcingHandler<T> where T : AggregateRootSimple
    {
        private readonly IEventStore _eventStore;
        private readonly IEventProducer _eventProducer;

        public EventSourcingHandler(IEventStore eventStore, IEventProducer eventProducer)
        {
            _eventStore = eventStore;
            _eventProducer = eventProducer;
        }

        public async Task<T> GetByIdAsync(Guid aggregateId)
        {
            var aggregate = Activator.CreateInstance(typeof(T));
            var events = await _eventStore.GetEventsAsync(aggregateId);

            if (events == null || !events.Any()) return (T)aggregate;

            ((T)aggregate).ReplayEvents(events);
            ((T)aggregate).Version = events.Select(x => x.Version).Max();

            return (T)aggregate;
        }

        public async Task RepublishEventsAsync(string exchangeKey, string routeKey, string queueKey)
        {
            var aggregateIds = await _eventStore.GetAggregateIdsAsync();

            if (aggregateIds is null || !aggregateIds.Any()) return;

            foreach (var aggregateId in aggregateIds)
            {
                var aggregate = await GetByIdAsync(aggregateId);

                if (aggregate is null) continue;

                var events = await _eventStore.GetEventsAsync(aggregateId);

                foreach (var @event in events)
                {
                    _eventProducer.Produce(exchangeKey, routeKey, queueKey, @event);
                }
            }
        }

        public async Task SaveAsync(AggregateRootSimple aggregate)
        {
            await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.GetType().Name, aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }
    }
}