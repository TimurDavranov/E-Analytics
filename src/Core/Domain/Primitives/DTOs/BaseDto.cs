using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Primitives.DTOs
{
    public abstract class BaseDto
    {
        protected BaseDto(long id)
        {
            Id = id;
        }

        protected BaseDto()
        {

        }

        public long Id { get; protected set; }
    }
}
