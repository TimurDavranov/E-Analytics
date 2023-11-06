using EAnalytics.Common.Configurations;
using EAnalytics.Common.Events;
using EAnalytics.Common.Helpers.RabbitAgent;
using Microsoft.Extensions.Options;
using OL.Application;
using OL.Parser.Infrastructure.Handlers;
using System.Text;
using System.Text.Json;

namespace OL.Parser.Infrastructure.Consumers
{
    public interface IOLConsumer
    {
        void Consume();
    }

    public class OLConsumer : IOLConsumer
    {
        private readonly IRabbitMessageConsumer _messageConsumer;
        private readonly IEventHandler _eventHandler;
        private readonly AppConfig _config;

        public OLConsumer(IRabbitMessageConsumer messageConsumer, IEventHandler eventHandler, IOptions<AppConfig> options)
        {
            _messageConsumer = messageConsumer;
            _eventHandler = eventHandler;
            _config = options.Value;
        }

        public void Consume()
        {
            _messageConsumer.Consume(_config.ExchangeKey, _config.RouteKey, _config.QueueKey, async (sender, args) =>
            {
                var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
                var message = JsonSerializer.Deserialize<BaseEvent>(Encoding.UTF8.GetString(args.Body.ToArray()), options);
                var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { message!.GetType() });
                if (handlerMethod is null)
                    throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method!");

                await (Task)handlerMethod.Invoke(_eventHandler, new object[] { message })!;
            }, 0, 1);
        }

    }
}
