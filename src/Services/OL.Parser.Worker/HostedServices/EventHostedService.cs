using OL.Parser.Infrastructure.Consumers;

namespace OL.Parser.Worker.HostedServices
{
    public class EventHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventHostedService> _logger;

        public EventHostedService(IServiceProvider serviceProvider, ILogger<EventHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var consumer = scope.ServiceProvider.GetRequiredService<IOLConsumer>();
                consumer.Consume();
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("OL event consumer is stoped");
            return Task.CompletedTask;
        }
    }
}
