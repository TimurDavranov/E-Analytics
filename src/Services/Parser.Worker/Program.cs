using EA.Application.Repositories;
using EA.Domain;
using EA.Domain.Abstraction.Repositories;
using EA.Infrastructure;
using EAnalytics.Common.Configurations;
using EAnalytics.Common.Helpers.RabbitAgent;
using Microsoft.EntityFrameworkCore;
using Parser.Worker.Consumers;
using Parser.Worker.Handlers;
using Parser.Worker.HostedServices;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.AddOptions<AppConfig>(builder, nameof(AppConfig));
        services.AddOptions<DataBaseConfiguration>(builder, nameof(DataBaseConfiguration));
        services.AddOptions<RabbitMQConfiguration>(builder, nameof(RabbitMQConfiguration));

        services.AddHostedService<EventHostedService>();

        services.AddScoped<IEAConsumer, EAConsumer>();
        services.AddScoped<IRabbitMessageConsumer, RabbitMessageConsumer>();
        services.AddScoped<IEventHandler, Parser.Worker.Handlers.EventHandler>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        var configuration = services
                .BuildServiceProvider()
                .GetService<IConfiguration>();
        services
            .AddDbContext<IEADbContext, EADbContext>(opt =>
            {
                opt.UseNpgsql(configuration!.GetConnectionString("DefaultConnection"));
            }, ServiceLifetime.Scoped);

        services
            .AddSingleton<IRabbitMessageProducer, RabbitMessageProducer>();
    })
    .Build();
host.Run();
