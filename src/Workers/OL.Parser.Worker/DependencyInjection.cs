using OL.Parser.Infrastructure;
using OL.Parser.Worker.HostedServices.Consumers;
using OL.Parser.Worker.HostedServices.Recurring;
using OL.Parser.Worker.Services;

namespace OL.Parser.Worker
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDI(this IServiceCollection services)
        {
            return services
                .AddInfrastructure()
                .AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<OLSystemService>()
                .AddScoped<CategoryCommandService>();
        }

        private static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            return services
                .AddHostedService<EventHostedService>()
                //.AddHostedService<ParseCategoryHostedService>()
                ;
        }
    }
}
