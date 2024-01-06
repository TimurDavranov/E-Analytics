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
                .AddServices()
                .AddHostedServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<OLSystemService>()
                .AddScoped<CategoryCommandService>()
                .AddScoped<ProductCommandService>()
                .AddScoped<CategoryQueryService>()
                .AddScoped<ProductQueryService>();
        }

        private static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            return services
                .AddHostedService<EventHostedService>()
                .AddHostedService<ParseProductHostedService>()
                .AddHostedService<ParseCategoryHostedService>();
        }
    }
}
