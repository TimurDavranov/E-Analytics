using EA.Application.Aggregates;
using EA.Application.Repositories;
using EA.Domain;
using EA.Domain.Abstraction.Repositories;
using EA.Domain.Events;
using EA.Infrastructure.Commands.Categories;
using EA.Infrastructure.Commands.Products;
using EA.Infrastructure.Dispatchers;
using EA.Infrastructure.Handlers;
using EA.Infrastructure.Stores;
using EAnalytics.Common.Commands;
using EAnalytics.Common.Factories;
using EAnalytics.Common.Helpers.RabbitAgent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace EA.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
            services
                .AddBsonMap()
                .AddDatabases()
                .AddRepositories()
                .AddServices()
                .AddHandlers();

        private static IServiceCollection AddRepositories(this IServiceCollection services) =>
            services
            .AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

        private static IServiceCollection AddServices(this IServiceCollection services) =>
            services
                .AddScoped<IEventStore, EventStore>()
                .AddScoped<IEventSourcingHandler<CategoryAggregateRoot>, EventSourcingHandler<CategoryAggregateRoot>>()
                .AddScoped<ICommandHandler, CommandHandler>();

        private static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            var commandHandler = services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
            var dispatcher = new CommandDispatcher();
            dispatcher.RegisterHandler<AddCategoryCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<EditCategoryCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<AddProductCommand>(commandHandler.HandleAsync);
            services.AddSingleton<ICommandDispatcher>(_ => dispatcher);
            return services;
        }

        private static IServiceCollection AddDatabases(this IServiceCollection services)
        {
            var configuration = services
                .BuildServiceProvider()
                .GetService<IConfiguration>();

            Action<DbContextOptionsBuilder> dbOptions = opt => opt.UseSqlServer(configuration!.GetConnectionString("DefaultConnection"));

            services
                .AddDbContext<IEADbContext, EADbContext>(dbOptions, ServiceLifetime.Scoped);
            services.AddSingleton(new DatabaseContextFactory<EADbContext>(dbOptions));
            services
                .AddSingleton<IRabbitMessageProducer, RabbitMessageProducer>();

            return services;
        }

        private static IServiceCollection AddBsonMap(this IServiceCollection service)
        {
            BsonClassMap.RegisterClassMap<AddCategoryEvent>();
            BsonClassMap.RegisterClassMap<EditCategoryEvent>();
            return service;
        }
    }
}