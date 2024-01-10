using EA.Parser.Infrastructure.Consumers;
using EA.Parser.Infrastructure.Handlers;
using EAnalytics.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EA.Parser.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services = EA.Infrastructure.DependencyInjection.AddInfrastructure(services);
            services
                .AddServices();
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IEAConsumer, EAConsumer>();
            services.AddScoped<IEventHandler, Handlers.EventHandler>();
            services.AddSingleton<IConcurencyControlService, ConcurencyControlService>();
            return services;
        }
    }
}