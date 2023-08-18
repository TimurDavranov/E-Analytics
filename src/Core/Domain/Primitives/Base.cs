using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Primitives
{
    public abstract class Base
    {
        protected Base(long id)
        {
            Id = id;
        }

        protected Base()
        {

        }

        public long Id { get; protected set; }
    }
}
