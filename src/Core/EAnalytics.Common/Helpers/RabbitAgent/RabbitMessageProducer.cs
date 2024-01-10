using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EAnalytics.Common.Configurations;
using EAnalytics.Common.Exceptions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace EAnalytics.Common.Helpers.RabbitAgent
{
    public interface IRabbitMessageProducer
    {
        bool IsConnected { get; }
        IModel Channel { get; }

        void Publish<T>(string exchange, string route, string queue, T message);
    }

    public class RabbitMessageProducer : IRabbitMessageProducer, IDisposable
    {

        public bool IsConnected { get; }
        public IModel Channel { get; }
        private readonly IConnection Connection;

        public RabbitMessageProducer(IRabbitConnection connectionService)
        {
            var props = connectionService.GetProperties();
            IsConnected = props.IsConnected;
            Channel = props.Channel;
            Connection = props.Connection;
        }

        public void Publish<T>(string exchange, string route, string queue, T message)
        {
            if (IsConnected)
            {
                Channel.ExchangeDeclare(exchange, ExchangeType.Direct);
                if (message is null)
                    throw new ArgumentNullException(nameof(message), "Message must have a value!");

                var json = JsonSerializer.Serialize(message, message.GetType());
                var body = Encoding.UTF8.GetBytes(json);

                var properties = Channel.CreateBasicProperties();
                properties.Persistent = true;

                Channel.QueueDeclare(queue, true, false, true);
                Channel.QueueBind(queue, exchange, route);

                Channel.BasicPublish(exchange, route, properties, body);
            }
            else throw new ConnectionRefusedException("Connection to RabbitMQ service is closed");
        }

        public void Dispose()
        {
        }
    }
}