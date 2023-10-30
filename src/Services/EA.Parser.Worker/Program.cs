using EA.Parser.Infrastructure;
using EA.Parser.Worker.HostedServices;
using EAnalytics.Common;
using EAnalytics.Common.Configurations;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {

        ConfigureOptions.AddOptions<AppConfig>(services, builder, nameof(AppConfig));
        ConfigureOptions.AddOptions<DataBaseConfiguration>(services, builder, nameof(DataBaseConfiguration));
        ConfigureOptions.AddOptions<RabbitMQConfiguration>(services, builder, nameof(RabbitMQConfiguration));
        ConfigureOptions.AddOptions<MongoDbConfiguration>(services, builder, nameof(MongoDbConfiguration));

        services.AddInfrastructure();

        services.AddHostedService<EventHostedService>();

    })
    .Build();
host.Run();
