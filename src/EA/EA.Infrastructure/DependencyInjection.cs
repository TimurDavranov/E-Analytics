using EA.Application.Aggregates;
using EA.Application.Repositories;
using EA.Domain;
using EA.Domain.Abstraction.Repositories;
using EA.Domain.Events;
using EA.Infrastructure.Commands.Categories;
using EA.Infrastructure.Dispatchers;
using EA.Infrastructure.Handlers;
using EA.Infrastructure.Stores;
using EAnalytics.Common.Commands;
using EAnalytics.Common.Helpers.RabbitAgent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace EA.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service) =>
            service
                .AddBsonMap()
                .AddDatabases()
                .AddRepositories()
                .AddServices()
                .AddHandlers();

        private static IServiceCollection AddRepositories(this IServiceCollection service) =>
            service
            .AddScoped(typeof(IRepository<>), typeof(Repository<>));

        private static IServiceCollection AddServices(this IServiceCollection service) =>
            service
                .AddScoped<IEventStore, EventStore>()
                .AddScoped<IEventSourcingHandler<CategoryAggregateRoot>, EventSourcingHandler<CategoryAggregateRoot>>()
                .AddScoped<ICommandHandler, CommandHandler>();

        private static IServiceCollection AddHandlers(this IServiceCollection service)
        {
            var commandHandler = service.BuildServiceProvider().GetRequiredService<ICommandHandler>();
            var dispatcher = new CommandDispatcher();
            dispatcher.RegisterHandler<AddCategoryCommand>(commandHandler.HandleAsync);
            service.AddSingleton<ICommandDispatcher>(_ => dispatcher);
            return service;
        }

        private static IServiceCollection AddDatabases(this IServiceCollection service)
        {
            var configuration = service
                .BuildServiceProvider()
                .GetService<IConfiguration>();

            service
                .AddDbContext<IEADbContext, EADbContext>(opt =>
                {
                    opt.UseNpgsql(configuration!.GetConnectionString("DefaultConnection"));
                }, ServiceLifetime.Scoped);

            service
                .AddSingleton<IRabbitMessageProducer, RabbitMessageProducer>();

            return service;
        }

        private static IServiceCollection AddBsonMap(this IServiceCollection service)
        {
            BsonClassMap.RegisterClassMap<AddCategoryEvent>();
            return service;
        }
    }
}