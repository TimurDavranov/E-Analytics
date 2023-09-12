using EA.Domain.Primitives;
using EA.Infrastructure.Stores;
using EAnalytics.Common.Aggregates;
using EAnalytics.Common.Producers;

namespace EA.Infrastructure.Handlers
{
    public interface IEventSourcingHandler<T>
    {
        Task SaveAsync(AggregateRootSimple aggregate);
        Task<T> GetByIdAsync(Guid aggregateId);
        Task RepublishEventsAsync();
    }

    public class EventSourcingHandler : IEventSourcingHandler<AggregateRootSimple>
    {
        private readonly IEventStore _eventStore;
        private readonly IEventProducer _eventProducer;

        public EventSourcingHandler(IEventStore eventStore, IEventProducer eventProducer)
        {
            _eventStore = eventStore;
            _eventProducer = eventProducer;
        }

        public Task<AggregateRootSimple> GetByIdAsync(Guid aggregateId)
        {
            throw new NotImplementedException();
        }

        public Task RepublishEventsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync(AggregateRootSimple aggregate)
        {
            await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }
    }
}