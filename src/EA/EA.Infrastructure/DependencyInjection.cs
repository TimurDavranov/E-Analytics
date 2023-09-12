using EA.Domain.Abstraction;
using EA.Domain.Primitives;
using EA.Infrastructure.Dispatchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EA.Infrastructure
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
            service;

        private static IServiceCollection AddHandlers(this IServiceCollection service)
        {
            //var commandHandler = service.BuildServiceProvider().GetRequiredService<ICommandHandler>();
            var dispatcher = new CommandDispatcher();
            //dispatcher.RegisterHandler<NewPostCommand>(commandHandler.HandleAsync);
            service.AddSingleton<ICommandDispatcher>(_ => dispatcher);
            return service;
        }

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