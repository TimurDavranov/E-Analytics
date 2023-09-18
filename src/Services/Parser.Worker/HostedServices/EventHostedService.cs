using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parser.Worker.Consumers;

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