using Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Primitives
{
    public abstract class BaseGuid : ISoftDelete
    {
        protected BaseGuid()
        {
        }

        protected BaseGuid(Guid id, bool deleted, DateTime? deletedAt, string deletedBy)
        {
            Id = id;
            Deleted = deleted;
            DeletedAt = deletedAt;
            DeletedBy = deletedBy;
        }

        public Guid Id { get; protected set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedBy { get; set; }
    }
}
