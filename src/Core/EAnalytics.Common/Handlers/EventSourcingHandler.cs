using EAnalytics.Common.Aggregates;
using EAnalytics.Common.Primitives.Models;
using EAnalytics.Common.Producers;
using EAnalytics.Common.Stores;

namespace EAnalytics.Common.Handlers
{
    public interface IEventSourcingHandler<TAggregateModel> where TAggregateModel : AggregateRootSimple
    {
        Task SaveAsync(AggregateRootSimple aggregate);
        Task<TAggregateModel> GetByIdAsync(Guid aggregateId);
        Task RepublishEventsAsync(string exchangeKey, string routeKey, string queueKey);
    }

    public class EventSourcingHandler<TAggregateModel> : IEventSourcingHandler<TAggregateModel> where TAggregateModel : AggregateRootSimple
    {
        private readonly IEventStore _eventStore;
        private readonly IEventProducer _eventProducer;

        public EventSourcingHandler(IEventStore eventStore, IEventProducer eventProducer)
        {
            _eventStore = eventStore;
            _eventProducer = eventProducer;
        }

        public async Task<TAggregateModel> GetByIdAsync(Guid aggregateId)
        {
            var aggregate = Activator.CreateInstance(typeof(TAggregateModel));
            var events = await _eventStore.GetEventsAsync(aggregateId);

            if (events == null || !events.Any()) return (TAggregateModel)aggregate;

            ((TAggregateModel)aggregate).ReplayEvents(events);
            ((TAggregateModel)aggregate).Version = events.Select(x => x.Version).Max();

            return (TAggregateModel)aggregate;
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