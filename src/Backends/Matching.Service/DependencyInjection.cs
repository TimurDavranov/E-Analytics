using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EAnalytics.Common.Factories;
using Microsoft.EntityFrameworkCore;
using OL.Domain;
using OL.Infrastructure;

namespace Matching.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDI(this IServiceCollection services)
        {
            return services
                .AddDatabases();
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

            return services;
        }
    }
}