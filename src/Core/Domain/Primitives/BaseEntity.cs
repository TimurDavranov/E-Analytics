using Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Primitives
{
    public abstract class BaseEntity<T> : IBaseEntity
    {
        public T Id { get; protected set; }

        public bool IsDeleted { get; set; }
    }
}
