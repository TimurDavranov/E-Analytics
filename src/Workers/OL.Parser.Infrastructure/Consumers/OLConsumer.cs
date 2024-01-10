using System.Collections.Concurrent;
using EAnalytics.Common.Configurations;
using EAnalytics.Common.Events;
using EAnalytics.Common.Helpers.RabbitAgent;
using Microsoft.Extensions.Options;
using OL.Application;
using OL.Parser.Infrastructure.Handlers;
using System.Text;
using System.Text.Json;
using EAnalytics.Common.Services;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;

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
        private readonly IOptions<RabbitMQConfiguration> _rabbitOptions;
        private readonly ILogger<OLConsumer> _logger;
        private readonly IConcurencyControlService _concurencyControlService;

        public OLConsumer(IRabbitMessageConsumer messageConsumer, IEventHandler eventHandler,
            IOptions<AppConfig> options, IOptions<RabbitMQConfiguration> rabbitOptions, ILogger<OLConsumer> logger,
            IConcurencyControlService concurencyControlService)
        {
            _messageConsumer = messageConsumer;
            _eventHandler = eventHandler;
            _config = options.Value;
            _rabbitOptions = rabbitOptions;
            _logger = logger;
            _concurencyControlService = concurencyControlService;
        }

        public void Consume()
        {
            _messageConsumer.Consume(_config.ExchangeKey, _config.RouteKey, _config.QueueKey,
                async (sender, args, channel) =>
                {
                    var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
                    var message =
                        JsonSerializer.Deserialize<BaseEvent>(Encoding.UTF8.GetString(args.Body.ToArray()),
                            options);
                    var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { message!.GetType() });
                    if (handlerMethod is null)
                        throw new ArgumentNullException(nameof(handlerMethod),
                            "Could not find event handler method!");

                    await (Task)handlerMethod.Invoke(_eventHandler, new object[] { message })!;
                }, 0, 1);
        }
    }
}