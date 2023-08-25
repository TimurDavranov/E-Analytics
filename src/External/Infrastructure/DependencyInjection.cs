using Application.Abstractions;
using Application.Configurations;
using Application.Shops.Queries.Olcha;
using Domain.Abstraction;
using Domain.Abstraction.Repositories.Olcha;
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
            service
                .AddScoped<IOlchaParserRepository, OlchaParserRepository>();

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

        public static IServiceCollection ConfigureAppSettings(this IServiceCollection service)
        {
            var configuration = service
                .BuildServiceProvider()
                .GetService<IConfiguration>();

            if (configuration == null)
                throw new ArgumentNullException(nameof(IConfiguration), "Wrong configurations!");

            service
                .Configure<AppConfig>(configuration.GetSection("Config"))
                .Configure<DataBaseConfiguration>(opt =>
                {
                    configuration.GetSection("DBConfig");
                });

            return service;
        }
    }
}
