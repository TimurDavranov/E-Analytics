using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EAnalytics.Common.Configurations;
using EAnalytics.Common.Exceptions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EAnalytics.Common.Helpers.RabbitAgent
{
    public interface IRabbitMessageConsumer
    {
        bool IsConnected { get; set; }
        IModel Channel { get; set; }
        void Consume(string exchangeKey, string routeKey, string queueKey, Action<object?, BasicDeliverEventArgs> action, uint prefetchSize, ushort prefetchCount);
        void Consume(string exchangeKey, string routeKey, string queueKey, Func<object?, BasicDeliverEventArgs, Task> action, uint prefetchSize, ushort prefetchCount);
    }

    public class RabbitMessageConsumer : IRabbitMessageConsumer, IDisposable
    {
        public bool IsConnected { get; set; } = false;
        public IModel Channel { get; set; }
        private ConnectionFactory _factory;
        private IConnection _connection;

        public RabbitMessageConsumer(IOptions<RabbitMQConfiguration> config)
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


        public void Consume(string exchangeKey, string routeKey, string queueKey, Action<object?, BasicDeliverEventArgs> action, uint prefetchSize, ushort prefetchCount)
        {
            if (IsConnected)
            {
                Channel.ExchangeDeclare(exchangeKey, ExchangeType.Direct);

                Channel.QueueDeclare(queueKey, false, false, true);

                Channel.QueueBind(queueKey, exchangeKey, routeKey);

                var consumer = new EventingBasicConsumer(Channel);
                consumer.Received += (sender, args) =>
                {
                    action.Invoke(sender, args);
                    Channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: true);
                };
                Channel.BasicQos(prefetchSize: prefetchSize, prefetchCount: prefetchCount, false);
                Channel.BasicConsume(queueKey, false, consumer);
            }
            else throw new ConnectionRefusedException("Connection to RabbitMQ service is closed");
        }

        public void Consume(string exchangeKey, string routeKey, string queueKey, Func<object?, BasicDeliverEventArgs, Task> action, uint prefetchSize, ushort prefetchCount)
        {
            if (IsConnected)
            {
                Channel.ExchangeDeclare(exchangeKey, ExchangeType.Direct);

                var queueName = Channel.QueueDeclare(queueKey, false, false, true).QueueName;

                Channel.QueueBind(queueName, exchangeKey, routeKey);

                var consumer = new EventingBasicConsumer(Channel);
                consumer.Received += async (sender, args) =>
                {
                    await action.Invoke(sender, args);
                    Channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: true);
                };
                Channel.BasicQos(prefetchSize: prefetchSize, prefetchCount: prefetchCount, false);
                Channel.BasicConsume(queueName, false, consumer);
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