using EAnalytics.Common;
using EAnalytics.Common.Configurations;
using OL.Parser.Infrastructure;
using OL.Parser.Worker.HostedServices.Consumers;

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
