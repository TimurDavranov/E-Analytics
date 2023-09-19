using System.Text;
using System.Text.Json;
using EA.Application.Converters;
using EAnalytics.Common.Configurations;
using EAnalytics.Common.Events;
using EAnalytics.Common.Helpers.RabbitAgent;
using Microsoft.Extensions.Options;
using Parser.Infrastructure.Handlers;

namespace Parser.Infrastructure.Consumers
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
        public EAConsumer(IRabbitMessageConsumer messageConsumer, IOptions<AppConfig> options, IEventHandler eventHandler)
        {
            _messageConsumer = messageConsumer;
            _config = options.Value;
            _eventHandler = eventHandler;
        }

        public void Consume()
        {
            _messageConsumer.Consume(_config.ExchangeKey, _config.RouteKey, _config.QueueKey, (sender, args) =>
            {
                var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
                var message = JsonSerializer.Deserialize<BaseEvent>(Encoding.UTF8.GetString(args.Body.ToArray()), options);
                var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { message.GetType() });

                if (handlerMethod is null)
                    throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method!");

                handlerMethod.Invoke(_eventHandler, new object[] { message });
            }, 0, 1);
        }
    }
}