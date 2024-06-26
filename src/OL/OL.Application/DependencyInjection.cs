using EAnalytics.Common.Abstractions.Repositories;
using EAnalytics.Common.Handlers;
using EAnalytics.Common.Helpers.RabbitAgent;
using EAnalytics.Common.Producers;
using EAnalytics.Common.Repositories;
using EAnalytics.Common.Stores;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using OL.Domain;

namespace OL.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddRepositories()
                .AddBsonMap();
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services
                .AddScoped(typeof(IRepository<,>), typeof(Repository<,>))
                .AddScoped<IEventStoreRepository, EventStoreRepository>()
                .AddScoped<IEventProducer, EventProducer>()
                .AddScoped<IEventStore, EventStore>()
                .AddScoped(typeof(IEventSourcingHandler<>), typeof(EventSourcingHandler<>))
                .AddSingleton<IRabbitMessageProducer, RabbitMessageProducer>()
                .AddSingleton<IRabbitMessageConsumer, RabbitMessageConsumer>()
                .AddSingleton<IRabbitConnection, RabbitConnection>();
            return services;
        }

        private static IServiceCollection AddBsonMap(this IServiceCollection services)
        {
            BsonClassMap.RegisterClassMap<AddOLCategoryEvent>();
            BsonClassMap.RegisterClassMap<AddOLProductEvent>();
            BsonClassMap.RegisterClassMap<EnableOLCategoryEvent>();
            BsonClassMap.RegisterClassMap<UpdateOLCategoryEvent>();
            BsonClassMap.RegisterClassMap<UpdateOlProductEvent>();
            return services;
        }
    }
}