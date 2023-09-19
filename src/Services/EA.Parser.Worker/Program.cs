using EA.Domain;
using EA.Infrastructure;
using EAnalytics.Common.Configurations;
using EAnalytics.Common.Helpers.RabbitAgent;
using Microsoft.EntityFrameworkCore;
using Parser.Worker.HostedServices;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.AddOptions<AppConfig>(builder, nameof(AppConfig));
        services.AddOptions<DataBaseConfiguration>(builder, nameof(DataBaseConfiguration));
        services.AddOptions<RabbitMQConfiguration>(builder, nameof(RabbitMQConfiguration));

        services.AddHostedService<EventHostedService>();

        services.AddInfrastructure();
    })
    .Build();
host.Run();
