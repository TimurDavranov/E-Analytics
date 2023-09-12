using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EA.Domain.Primitives.Base
{
    public abstract class BaseCommand
    {
        public Guid Id { get; set; }
    }
}