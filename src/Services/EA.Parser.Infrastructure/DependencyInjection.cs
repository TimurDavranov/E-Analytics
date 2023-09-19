using EA.Application.Repositories;
using EA.Domain;
using EA.Domain.Abstraction.Repositories;
using EA.Infrastructure;
using EAnalytics.Common.Helpers.RabbitAgent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parser.Infrastructure.Consumers;
using Parser.Infrastructure.Handlers;

namespace Parser.Infrastructure
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
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IEAConsumer, EAConsumer>();
            services.AddScoped<IRabbitMessageConsumer, RabbitMessageConsumer>();
            services.AddScoped<IEventHandler, Handlers.EventHandler>();

            services.AddSingleton<IRabbitMessageProducer, RabbitMessageProducer>();

            return services;
        }

        private static IServiceCollection AddDatabases(this IServiceCollection services)
        {
            var configuration = services
                .BuildServiceProvider()
                .GetService<IConfiguration>();
            services
                .AddDbContext<IEADbContext, EADbContext>(opt =>
                {
                    opt.UseSqlServer(configuration!.GetConnectionString("DefaultConnection"));
                }, ServiceLifetime.Scoped);

            return services;
        }
    }
}