using Application.Repositories;
using EA.Application.Aggregates;
using EA.Application.Exceptions;
using EA.Domain.Primitives.Models;
using EAnalytics.Common.Events;
using EAnalytics.Common.Producers;

namespace EA.Infrastructure.Stores
{
    public interface IEventStore
    {
        Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
        Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
        Task<List<Guid>> GetAggregateIdsAsync();
    }

    public class EventStore : IEventStore
    {
        private readonly IEventProducer _eventProducer;
        private readonly IEventStoreRepository _eventStoreRepository;
        public EventStore(IEventProducer eventProducer, IEventStoreRepository eventStoreRepository)
        {
            _eventProducer = eventProducer;
            _eventStoreRepository = eventStoreRepository;
        }

        public Task<List<Guid>> GetAggregateIdsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
        {
            throw new NotImplementedException();
        }

        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

            if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
                throw new ConcurrencyException();

            var version = expectedVersion;

            foreach (var @event in events)
            {
                version++;
                @event.Version = version;
                var eventType = @event.GetType().Name;
                var eventModel = new EventModel
                {
                    TimeStamp = DateTime.Now,
                    AggregateIdentifier = aggregateId,
                    AggregateType = nameof(CategoryAggregateRoot),
                    Version = version,
                    EventType = eventType,
                    EventData = @event
                };

                await _eventStoreRepository.SaveAsync(eventModel);

                //await _eventProducer.ProduceAsync(topic, @event);
            }
        }
    }
}