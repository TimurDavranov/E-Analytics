using EA.Domain;
using EA.Infrastructure;
using EAnalytics.Common.Configurations;
using EAnalytics.Common.Factories;
using EAnalytics.Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using OL.Domain;
using OL.Infrastructure;

namespace Matching.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDI(this IServiceCollection services)
        {
            return services
                .AddDatabases()
                .AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IMinioService, MinioService>();
        }

        private static IServiceCollection AddDatabases(this IServiceCollection services)
        {
            var configuration = services
                            .BuildServiceProvider()
                            .GetService<IConfiguration>();

            Action<DbContextOptionsBuilder> dbOptions = opt =>
                opt
                    .UseSqlServer(configuration!.GetConnectionString("DefaultConnection"),
                        o => o.EnableRetryOnFailure())
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();

            services
                .AddDbContext<IOLDbContext, OLDbContext>(dbOptions, ServiceLifetime.Scoped);
            services.AddSingleton(new DatabaseContextFactory<OLDbContext>(dbOptions));
            services
                .AddDbContext<IEADbContext, EADbContext>(dbOptions, ServiceLifetime.Scoped);
            services.AddSingleton(new DatabaseContextFactory<EADbContext>(dbOptions));

            return services;
        }
    }
}