using Application.Abstractions;
using Infrastructure.Repositories.Olcha.uz;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service)
        {
            service.AddScoped<IParserRepository, OlchaParserRepository>();
            return service;
        }
    }
}
