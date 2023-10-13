using EA.Parser.Infrastructure;
using EA.Parser.Worker.HostedServices;
using EAnalytics.Common.Configurations;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {

        EA.Infrastructure.ConfigureOptions.AddOptions<AppConfig>(services, builder, nameof(AppConfig));
        EA.Infrastructure.ConfigureOptions.AddOptions<DataBaseConfiguration>(services, builder, nameof(DataBaseConfiguration));
        EA.Infrastructure.ConfigureOptions.AddOptions<RabbitMQConfiguration>(services, builder, nameof(RabbitMQConfiguration));

        services.AddInfrastructure();

        services.AddHostedService<EventHostedService>();

    })
    .Build();
host.Run();
