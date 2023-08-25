using Application.Abstractions;
using Application.Constants;
using Domain.Abstraction.Repositories.Olcha;
using Infrastructure.Repositories.Olcha;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service) =>
            service
            .AddRepositories()
            .AddServices();

        private static IServiceCollection AddRepositories(this IServiceCollection service) =>
            service
            .AddScoped<IOlchaParserRepository, OlchaParserRepository>();

        private static IServiceCollection AddServices(this IServiceCollection service) =>
            service
            .AddScoped<IOlchaConnectionService, OlchaConnectionService>()
            .AddSingleton<AppConfig>();
    }
}
