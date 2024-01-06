using EAnalytics.Common.Abstractions.Repositories;
using EAnalytics.Common.Dispatchers;
using EAnalytics.Common.Factories;
using EAnalytics.Common.Queries;
using EAnalytics.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OL.Domain;
using OL.Infrastructure;
using OL.Infrastructure.Models.Requests.Category;
using OL.Infrastructure.Models.Requests.Product;
using OL.Infrastructure.Models.Responses.Category;
using OL.Infrastructure.Models.Responses.Product;
using OL.Query.Api.Infrastructure.Handlers;

namespace OL.Query.Api.Infrastructure;

public static class DependensyInjecttion
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
                .AddDatabases()
                .AddServices()
                .AddHandlers();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped(typeof(IRepository<,>), typeof(Repository<,>))
            .AddScoped<IQueryHandler, QueryHandler>();
        return services;
    }

    private static IServiceCollection AddDatabases(this IServiceCollection services)
    {
        var configuration = services
                .BuildServiceProvider()
                .GetService<IConfiguration>();

        Action<DbContextOptionsBuilder> dbOptions = opt => opt.UseSqlServer(configuration!.GetConnectionString("DefaultConnection"));

        services
            .AddDbContext<IOLDbContext, OLDbContext>(dbOptions, ServiceLifetime.Scoped);
        services.AddSingleton(new DatabaseContextFactory<OLDbContext>(dbOptions));

        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        var queryHandler = services.BuildServiceProvider().GetRequiredService<IQueryHandler>();
        var queryDispatcher = new QueryDispatcher();
        queryDispatcher.RegisterHandler<CategoryByIdRequest, CategoryResponse>(queryHandler.HandleAsync);
        queryDispatcher.RegisterHandler<CategoryBySystemIdRequest, CategoryResponse>(queryHandler.HandleAsync);
        queryDispatcher.RegisterHandler<GetAllRequest, GetAllResponse<CategoryIdsResponse>>(queryHandler.HandleAsync);
        
        queryDispatcher.RegisterHandler<ProductBySystemIdRequest, ProductResponse>(queryHandler.HandleAsync);
        
        services.AddSingleton<IQueryDispatcher>(_ => queryDispatcher);
        return services;
    }
}
