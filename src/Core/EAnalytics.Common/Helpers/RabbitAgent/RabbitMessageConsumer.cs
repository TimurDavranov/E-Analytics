using System;
using System.Collections.Concurrent;
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
        bool IsConnected { get; }
        IModel Channel { get; }
        void Consume(string exchangeKey, string routeKey, string queueKey, Action<object?, BasicDeliverEventArgs, IModel> action, uint prefetchSize, ushort prefetchCount);
        void Consume(string exchangeKey, string routeKey, string queueKey, Func<object?, BasicDeliverEventArgs, IModel, Task> action, uint prefetchSize, ushort prefetchCount);
    }

    public class RabbitMessageConsumer : IRabbitMessageConsumer, IDisposable
    {
        public bool IsConnected { get; }
        public IModel Channel { get; }
        private readonly IConnection Connection;

        public RabbitMessageConsumer(IRabbitConnection connectionService)
        {
            var props = connectionService.GetProperties();
            IsConnected = props.IsConnected;
            Channel = props.Channel;
            Connection = props.Connection;
        }

        public void Consume(string exchangeKey, string routeKey, string queueKey, Action<object?, BasicDeliverEventArgs, IModel> action, uint prefetchSize, ushort prefetchCount)
        {
            if (IsConnected)
            {
                Channel.ExchangeDeclare(exchangeKey, ExchangeType.Direct);

                Channel.QueueDeclare(queueKey, true, false, true);

                Channel.QueueBind(queueKey, exchangeKey, routeKey);

                var consumer = new EventingBasicConsumer(Channel);
                consumer.Received += (sender, args) =>
                {
                    action.Invoke(sender, args, Channel);
                };
                Channel.BasicQos(prefetchSize: prefetchSize, prefetchCount: prefetchCount, false);
                Channel.BasicConsume(queueKey, false, consumer);
            }
            else throw new ConnectionRefusedException("Connection to RabbitMQ service is closed");
        }

        private int counterReconnections = 1;
        public void Consume(string exchangeKey, string routeKey, string queueKey, Func<object?, BasicDeliverEventArgs, IModel, Task> action, uint prefetchSize, ushort prefetchCount)
        {
            if (IsConnected)
            {
                Channel.ExchangeDeclare(exchangeKey, ExchangeType.Direct);

                var queueName = Channel.QueueDeclare(queueKey, true, false, true).QueueName;

                Channel.QueueBind(queueName, exchangeKey, routeKey);

                var consumer = new EventingBasicConsumer(Channel);
                consumer.Received += async (sender, args) =>
                {
                    await action.Invoke(sender, args, Channel);
                    Channel.BasicAck(args.DeliveryTag, true);
                };
                Channel.BasicQos(prefetchSize: prefetchSize, prefetchCount: prefetchCount, false);
                Channel.BasicConsume(queueName, false, consumer);
            }
            else throw new ConnectionRefusedException("Connection to RabbitMQ service is closed");
        }

        public void Dispose()
        {
        }
    }
}