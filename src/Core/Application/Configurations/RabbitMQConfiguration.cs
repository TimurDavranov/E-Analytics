using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Configurations;

public class RabbitMQConfiguration
{
    public string RabbitMQConnection { get; set; }
    public string RabbitMQUser { get; set; }
    public string RabbitMQPassword { get; set; }
    public int RabbitMQPort { get; set; }
    public string RabbitMQVirtualHost { get; set; }
}