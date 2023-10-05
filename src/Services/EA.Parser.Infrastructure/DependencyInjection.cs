using EA.Application.Repositories;
using EA.Domain;
using EA.Domain.Abstraction.Repositories;
using EA.Infrastructure;
using EAnalytics.Common.Configurations;
using EAnalytics.Common.Helpers.RabbitAgent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Parser.Infrastructure.Consumers;
using Parser.Infrastructure.Handlers;

namespace Parser.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, HostBuilderContext builder)
        {
            services = EA.Infrastructure.DependencyInjection.AddInfrastructure(services);
            services
                .AddRepositories()
                .AddServices()
                .ConfigureOptions(builder);
            return services;
        }
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }

        private static IServiceCollection ConfiguteOptions(this IServiceCollection services, HostBuilderContext builder)
        {
            services.AddOptions<AppConfig>(builder, nameof(AppConfig));
            services.AddOptions<DataBaseConfiguration>(builder, nameof(DataBaseConfiguration));
            services.AddOptions<RabbitMQConfiguration>(builder, nameof(RabbitMQConfiguration));
            services.AddOptions<MongoDbConfiguration>(builder, nameof(MongoDbConfiguration));
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IEAConsumer, EAConsumer>();
            services.AddScoped<IRabbitMessageConsumer, RabbitMessageConsumer>();
            services.AddScoped<IEventHandler, Handlers.EventHandler>();

            return services;
        }
    }
}