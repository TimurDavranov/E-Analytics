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
        bool IsConnected { get; set; }
        IModel Channel { get; set; }

        void Publish<T>(string exchange, string route, string queue, T message);
    }

    public class RabbitMessageProducer : IRabbitMessageProducer, IDisposable
    {

        public bool IsConnected { get; set; } = false;
        public IModel Channel { get; set; }
        private ConnectionFactory _factory;
        private IConnection _connection;

        public RabbitMessageProducer(IOptions<RabbitMQConfiguration> config)
        {
            if (config.Value is null)
                throw new ArgumentNullException(nameof(config), "RabbitMQ options is empty");

            if (string.IsNullOrEmpty(config.Value.RabbitMQUrl))
                _factory = new ConnectionFactory
                {
                    HostName = config.Value.RabbitMQConnection,
                    Password = config.Value.RabbitMQPassword,
                    UserName = config.Value.RabbitMQUser,
                    Port = config.Value.RabbitMQPort ?? 5672,
                    VirtualHost = config.Value.RabbitMQVirtualHost,
                };
            else
                _factory = new ConnectionFactory
                {
                    Uri = new Uri(config.Value.RabbitMQUrl)
                };

            _connection = _factory.CreateConnection();
            Channel = _connection.CreateModel();
            if (_connection.IsOpen) IsConnected = true;
            else throw new ConnectionRefusedException("Can't connect to RabbitMQ service");
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

                Channel.QueueDeclare(queue, false, false, true);
                Channel.QueueBind(queue, exchange, route);

                Channel.BasicPublish(exchange, route, properties, body);
            }
            else throw new ConnectionRefusedException("Connection to RabbitMQ service is closed");
        }

        public void Dispose()
        {
            if (_connection.IsOpen)
                _connection.Close();
        }
    }
}