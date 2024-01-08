using EAnalytics.Common;
using EAnalytics.Common.Configurations;
using EAnalytics.Common.Helpers;
using OL.Parser.Worker;
using Serilog;

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
    .ConfigureLogging((c, b) => b.ConfigureSerilog(c.Configuration, c.HostingEnvironment.ApplicationName))
    .Build();

host.Run();
