using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Primitives
{
    public abstract class BaseGuid
    {
        protected BaseGuid(Guid id)
        {
            Id = id;
        }
        protected BaseGuid()
        {
        }

        public Guid Id { get; protected set; }
    }
}
