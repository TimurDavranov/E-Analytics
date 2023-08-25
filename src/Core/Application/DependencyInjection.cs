using Application.Abstractions;
using Application.Shops.Queries.Olcha;
using Domain.Abstraction.Repositories.Olcha;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

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
        .AddScoped<IOlchaParserRepository, OlchaParserRepository>()
        .AddScoped<IOlchaCategoryReadRepository, OlchaCategoryReadRepository>();

}
