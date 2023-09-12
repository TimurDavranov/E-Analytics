using System.Text;
using EA.Application.Configurations;
using EA.Application.Exceptions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace EA.Application.Extentions.RabbitAgent
{
    public interface IRabbitMessageProducer
    {
        bool IsConnected { get; set; }
        IModel Channel { get; set; }

        void Publish(string exchange, string route, string message);
        void Connect();
        void CloseConnection();
    }

    public class RabbitMessageProducer : IRabbitMessageProducer
    {

        public bool IsConnected { get; set; } = false;
        public IModel Channel { get; set; }
        private readonly RabbitMQConfiguration _config;
        private ConnectionFactory _factory;
        private IConnection _connection;

        public RabbitMessageProducer(IOptions<RabbitMQConfiguration> config)
        {
            _config = config.Value;
        }

        public void Connect()
        {
            _factory = new ConnectionFactory
            {
                HostName = _config.RabbitMQConnection,
                Password = _config.RabbitMQPassword,
                UserName = _config.RabbitMQUser,
                Port = _config.RabbitMQPort ?? 5672,
                VirtualHost = _config.RabbitMQVirtualHost,
            };

            _connection = _factory.CreateConnection();
            Channel = _connection.CreateModel();

            if (_connection.IsOpen) IsConnected = true;
            else throw new ConnectionRefusedException("Can't connect to RabbitMQ service");
        }

        public void CloseConnection()
        {
            if (_connection.IsOpen)
            {
                _connection.Close();
            }
        }

        public void Publish(string exchange, string route, string message)
        {
            if (IsConnected)
            {
                Channel.ExchangeDeclare(exchange, ExchangeType.Direct);

                var body = Encoding.UTF8.GetBytes(message);

                var properties = Channel.CreateBasicProperties();
                properties.Persistent = true;

                Channel.BasicPublish(exchange, route, properties, body);
            }
            else throw new ConnectionRefusedException("Connection to RabbitMQ service is closed");
        }
    }
}