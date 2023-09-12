using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Domain.Primitives.DTOs
{
    public abstract class BaseDto<T>
    {
        protected BaseDto(T id)
        {
            Id = id;
        }

        protected BaseDto()
        {

        }

        public T Id { get; protected set; }
    }
}
