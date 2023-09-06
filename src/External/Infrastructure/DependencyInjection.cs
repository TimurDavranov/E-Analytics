using Application.Configurations;
using Domain.Abstraction;
using Domain.Abstraction.Services;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service) =>
            service
                .AddRepositories()
                .AddServices()
                .AddDatabases();

        private static IServiceCollection AddRepositories(this IServiceCollection service) =>
            service;

        private static IServiceCollection AddServices(this IServiceCollection service) =>
            service
                .AddScoped<IOlchaConnectionService, OlchaConnectionService>()
                .AddSingleton<AppConfig>();

        private static IServiceCollection AddDatabases(this IServiceCollection service)
        {
            var configuration = service
                .BuildServiceProvider()
                .GetService<IConfiguration>();

            service
                .AddDbContext<IApplicationDbContext, ApplicationDbContext>(opt =>
                {
                    opt.UseNpgsql(configuration!.GetConnectionString("DefaultConnection"));
                }, ServiceLifetime.Scoped);

            return service;
        }
    }
}