using EAnalytics.Common;
using EAnalytics.Common.Configurations;
using OL.Parser.Infrastructure;
using OL.Parser.Worker.HostedServices.Consumers;
using OL.Parser.Worker.HostedServices.Recurring;
using OL.Parser.Worker.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.AddHttpClient();

        ConfigureOptions.AddOptions<AppConfig>(services, builder, nameof(AppConfig));
        ConfigureOptions.AddOptions<DataBaseConfiguration>(services, builder, nameof(DataBaseConfiguration));
        ConfigureOptions.AddOptions<RabbitMQConfiguration>(services, builder, nameof(RabbitMQConfiguration));
        ConfigureOptions.AddOptions<MongoDbConfiguration>(services, builder, nameof(MongoDbConfiguration));

        services.AddInfrastructure();

        services.AddScoped<OLSystemService>();
        services.AddScoped<WebApiGetwayService>();

        services.AddHostedService<EventHostedService>();
        services.AddHostedService<ParseCategoryHostedService>();
    })
    .Build();

host.Run();
