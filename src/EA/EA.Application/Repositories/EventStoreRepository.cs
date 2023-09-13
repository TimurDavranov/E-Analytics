using EA.Domain.Primitives.Models;
using EAnalytics.Common.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Application.Repositories;

public interface IEventStoreRepository
{
    Task SaveAsync(EventModel @event);
    Task<List<EventModel>> FindByAggregateId(Guid aggregateId);
    Task<List<EventModel>> FindAllAsync();
}

public class EventStoreRepository : IEventStoreRepository
{
    private readonly IMongoCollection<EventModel> _eventStoreCollection;

    public EventStoreRepository(IOptions<MongoDbConfiguration> config)
    {
        var mongoClient = new MongoClient(config.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);

        _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection); ;
    }

    public Task<List<EventModel>> FindAllAsync()
    {
        return _eventStoreCollection.Find(_ => true).ToListAsync();
    }

    public Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
    {
        return _eventStoreCollection.Find(x => x.AggregateIdentifier == aggregateId).ToListAsync();
    }

    public Task SaveAsync(EventModel @event)
    {
        return _eventStoreCollection.InsertOneAsync(@event);
    }
}