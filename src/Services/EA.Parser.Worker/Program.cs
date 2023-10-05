using EA.Application;
using Parser.Infrastructure;
using Parser.Worker.HostedServices;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.AddHostedService<EventHostedService>();

        services
            .AddApplication()
            .AddInfrastructure();
    })
    .Build();
host.Run();
