using EA.Infrastructure;
using EAnalytics.Common.Configurations;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.AddOptions<AppConfig>(builder, nameof(AppConfig));
        services.AddOptions<DataBaseConfiguration>(builder, nameof(DataBaseConfiguration));
    })
    .Build();
host.Run();
