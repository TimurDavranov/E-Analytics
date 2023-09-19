using Parser.Infrastructure.Consumers;

namespace Parser.Worker.HostedServices
{
    public class EventHostedService : IHostedService
    {
        private readonly IEAConsumer _consumer;
        public EventHostedService(IEAConsumer consumer)
        {
            _consumer = consumer;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Consume();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}