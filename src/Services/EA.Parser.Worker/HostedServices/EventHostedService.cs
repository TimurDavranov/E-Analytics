using EA.Parser.Infrastructure.Consumers;

namespace EA.Parser.Worker.HostedServices
{
    public class EventHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public EventHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var consumer = scope.ServiceProvider.GetRequiredService<IEAConsumer>();
                consumer.Consume();
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}