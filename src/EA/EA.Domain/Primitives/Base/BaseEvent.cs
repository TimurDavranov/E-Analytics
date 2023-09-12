using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EA.Domain.Primitives.Base
{
    public class BaseEvent
    {
        protected BaseEvent(string type)
        {
            Type = type;
        }

        public Guid Id { get; set; }
        public string Type { get; set; }
        public int Version { get; set; }
    }
}