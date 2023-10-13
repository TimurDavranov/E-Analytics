using EA.Application.Repositories;
using EA.Domain;
using EA.Domain.Abstraction.Repositories;
using EA.Infrastructure;
using EA.Parser.Infrastructure.Consumers;
using EA.Parser.Infrastructure.Handlers;
using EAnalytics.Common.Factories;
using EAnalytics.Common.Helpers.RabbitAgent;
using EAnalytics.Common.Producers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EA.Parser.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
            services
                .AddDatabases()
                .AddRepositories()
                .AddServices();
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMessageProducer, RabbitMessageProducer>();
            services.AddSingleton<IRabbitMessageConsumer, RabbitMessageConsumer>();
            services.AddScoped<IEAConsumer, EAConsumer>();
            services.AddScoped<IEventHandler, Handlers.EventHandler>();
            services.AddScoped<IEventProducer, EventProducer>();
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