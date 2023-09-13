using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EA.Application
{
    public struct RabbitMQKeys
    {
        public const string EventExchange = "event-exchange";
        public const string EventQueueRoute = "event-queue-route";
        public const string EventQueue = "event-queue";
    }
}