using Application.Repositories;
using EA.Application;
using EA.Application.Exceptions;
using EA.Domain.Primitives.Models;
using EA.Infrastructure.Producers;
using EAnalytics.Common.Configurations;
using EAnalytics.Common.Events;
using EAnalytics.Common.Exceptions;
using Microsoft.Extensions.Options;

namespace EA.Infrastructure.Stores
{
    public interface IEventStore
    {
        Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, string aggregateTypeName, int expectedVersion);
        Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
        Task<List<Guid>> GetAggregateIdsAsync();
    }

    public class EventStore : IEventStore
    {
        private readonly IEventProducer _eventProducer;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly AppConfig _config;
        public EventStore(IEventProducer eventProducer, IEventStoreRepository eventStoreRepository, IOptions<AppConfig> options)
        {
            _eventProducer = eventProducer;
            _eventStoreRepository = eventStoreRepository;
            _config = options.Value;
        }

        public async Task<List<Guid>> GetAggregateIdsAsync()
        {
            var eventStream = await _eventStoreRepository.FindAllAsync();

            if (eventStream == null || !eventStream.Any())
                throw new ArgumentNullException(nameof(eventStream), "Could not retrieve event stream from the event store!");

            return eventStream.Select(x => x.AggregateIdentifier).Distinct().ToList();
        }

        public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
        {
            var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

            if (eventStream == null || !eventStream.Any())
                throw new AggregateNotFoundException("Incorrect aggregate ID provided!");

            return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
        }

        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, string aggregateTypeName, int expectedVersion)
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
                    AggregateType = aggregateTypeName,
                    Version = version,
                    EventType = eventType,
                    EventData = @event
                };

                await _eventStoreRepository.SaveAsync(eventModel);

                _eventProducer.Produce(_config.ExchangeKey, _config.RouteKey, @event);
            }
        }
    }
}