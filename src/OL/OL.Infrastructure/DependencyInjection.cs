using EAnalytics.Common.Commands;
using EAnalytics.Common.Dispatchers;
using EAnalytics.Common.Factories;
using EAnalytics.Common.Helpers.RabbitAgent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OL.Application;
using OL.Domain;
using OL.Infrastructure.Commands.Categories;
using OL.Infrastructure.Handlers;
using OL.Infrastructure.Models.Requests.Category;
using OL.Infrastructure.Models.Responses.Category;

namespace OL.Infrastructure;

public static class DependencyInjectio
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddApplication()
            .AddDatabases()
            .AddServices()
            .AddHandlers();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler, CommandHandler>();
        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        var commandHandler = services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
        var commandDispatcher = new CommandDispatcher();
        commandDispatcher.RegisterHandler<AddOLCategoryCommand>(commandHandler.HandleAsync);
        commandDispatcher.RegisterHandler<EnableOLCategoryCommand>(commandHandler.HandleAsync);
        services.AddSingleton<ICommandDispatcher>(_ => commandDispatcher);

        //var queryHandler = services.BuildServiceProvider().GetRequiredService<IQueryHandler>();
        //var queryDispatcher = new QueryDispatcher();
        //queryDispatcher.RegisterHandler<CategoryResponse, CategoryByIdRequest>(queryHandler.HandleAsync);

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
}
