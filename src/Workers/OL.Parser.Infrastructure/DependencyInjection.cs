using EAnalytics.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using OL.Parser.Infrastructure.Consumers;
using OL.Parser.Infrastructure.Handlers;

namespace OL.Parser.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services = OL.Infrastructure.DependencyInjectio.AddInfrastructure(services);
            services
                .AddServices();
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IOLConsumer, OLConsumer>();
            services.AddScoped<IEventHandler, Handlers.EventHandler>();
            services.AddSingleton<IConcurencyControlService, ConcurencyControlService>();
            return services;
        }
    }
}
