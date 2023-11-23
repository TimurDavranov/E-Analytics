using EAnalytics.Common;
using EAnalytics.Common.Configurations;
using OL.Parser.Worker;
Console.Title = System.Reflection.Assembly.GetExecutingAssembly().FullName ?? string.Empty;
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.AddHttpClient();

        ConfigureOptions.AddOptions<AppConfig>(services, builder, nameof(AppConfig));
        ConfigureOptions.AddOptions<DataBaseConfiguration>(services, builder, nameof(DataBaseConfiguration));
        ConfigureOptions.AddOptions<RabbitMQConfiguration>(services, builder, nameof(RabbitMQConfiguration));
        ConfigureOptions.AddOptions<MongoDbConfiguration>(services, builder, nameof(MongoDbConfiguration));

        services.AddDI();
    })
    .Build();

host.Run();
