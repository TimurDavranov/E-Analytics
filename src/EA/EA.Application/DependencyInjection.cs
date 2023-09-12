using EA.Application.Repositories;
using EA.Domain.Abstraction.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EA.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        return service
            .AddServices()
            .AddHttpClient();
    }

    private static IServiceCollection AddServices(this IServiceCollection services) =>
        services
        .AddScoped(typeof(IRepository<>), typeof(Repository<>));

}
