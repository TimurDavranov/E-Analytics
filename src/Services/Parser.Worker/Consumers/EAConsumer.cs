using System.Text;
using System.Text.Json;
using EA.Application.Converters;
using EAnalytics.Common.Configurations;
using EAnalytics.Common.Events;
using EAnalytics.Common.Helpers.RabbitAgent;
using Microsoft.Extensions.Options;
using Parser.Worker.Handlers;

namespace Parser.Worker.Consumers
{
    public interface IEAConsumer
    {
        void Consume();
    }

    public class EAConsumer : IEAConsumer
    {
        private readonly IRabbitMessageConsumer _messageConsumer;
        private readonly IEventHandler _eventHandler;
        private readonly AppConfig _config;
        public EAConsumer(IRabbitMessageConsumer messageConsumer, IOptions<AppConfig> options)
        {
            _messageConsumer = messageConsumer;
            _config = options.Value;
        }

        public void Consume()
        {
            _messageConsumer.Consume(_config.ExchangeKey, _config.RouteKey, _config.QueueKey, (sender, args) =>
            {
                var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
                var message = JsonSerializer.Deserialize<BaseEvent>(Encoding.UTF8.GetString(args.Body.ToArray()), options);
                var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { @message.GetType() });
            }, 0, 1);
        }
    }
}