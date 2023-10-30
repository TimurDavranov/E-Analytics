using EA.Application;
using EA.Application.Aggregates;
using EA.Domain;
using EA.Domain.Events;
using EA.Infrastructure.Commands.Categories;
using EA.Infrastructure.Commands.Products;
using EA.Infrastructure.Handlers;
using EAnalytics.Common.Commands;
using EAnalytics.Common.Dispatchers;
using EAnalytics.Common.Factories;
using EAnalytics.Common.Handlers;
using EAnalytics.Common.Helpers.RabbitAgent;
using EAnalytics.Common.Stores;
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
                .AddDatabases()
                .AddApplication()
                .AddServices()
                .AddHandlers();

        private static IServiceCollection AddServices(this IServiceCollection services) =>
            services
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

            return services;
        }
    }
}