using EA.Domain.Events;
using EAnalytics.Common.Abstractions.Repositories;
using EAnalytics.Common.Handlers;
using EAnalytics.Common.Helpers.RabbitAgent;
using EAnalytics.Common.Producers;
using EAnalytics.Common.Repositories;
using EAnalytics.Common.Stores;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace EA.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        return service
            .AddServices()
            .AddBsonMap()
            .AddHttpClient();
    }

    private static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddScoped(typeof(IRepository<,>), typeof(Repository<,>))
            .AddScoped<IEventStoreRepository, EventStoreRepository>()
            .AddScoped<IEventProducer, EventProducer>()
            .AddScoped<IEventStore, EventStore>()
            .AddScoped(typeof(IEventSourcingHandler<>), typeof(EventSourcingHandler<>))
            .AddSingleton<IRabbitMessageProducer, RabbitMessageProducer>()
            .AddSingleton<IRabbitMessageConsumer, RabbitMessageConsumer>();

    private static IServiceCollection AddBsonMap(this IServiceCollection services)
    {
        BsonClassMap.RegisterClassMap<AddCategoryEvent>();
        BsonClassMap.RegisterClassMap<AddProductEvent>();
        BsonClassMap.RegisterClassMap<EditCategoryEvent>();
        return services;
    }

}
