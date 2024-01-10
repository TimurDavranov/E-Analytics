using EAnalytics.Common.Configurations;
using EAnalytics.Common.Exceptions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace EAnalytics.Common.Helpers.RabbitAgent;

public record RabbitConnectionProperties(bool IsConnected, IModel Channel, IConnection Connection);

public interface IRabbitConnection
{
    RabbitConnectionProperties GetProperties();
}

public class RabbitConnection(IOptions<RabbitMQConfiguration> config) : IRabbitConnection
{
    private bool _isConnected;
    private IModel _channel;
    private IConnection _connection;
    private ConnectionFactory _factory;

    private void Connect()
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
        _channel = _connection.CreateModel();
        if (_connection.IsOpen) _isConnected = true;
        else throw new ConnectionRefusedException("Can't connect to RabbitMQ service");
    }

    public RabbitConnectionProperties GetProperties()
    {
        Connect();
        return new RabbitConnectionProperties(_isConnected, _channel, _connection);
    }
}
