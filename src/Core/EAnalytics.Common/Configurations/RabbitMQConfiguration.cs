namespace EAnalytics.Common.Configurations;

public class RabbitMQConfiguration
{
    public string RabbitMQConnection { get; set; }
    public string RabbitMQUser { get; set; }
    public string RabbitMQPassword { get; set; }
    public int? RabbitMQPort { get; set; }
    public string RabbitMQVirtualHost { get; set; }
    public string RabbitMQUrl { get; set; }
}