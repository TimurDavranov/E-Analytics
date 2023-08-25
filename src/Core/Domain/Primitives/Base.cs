using Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Primitives
{
    public abstract class Base : ISoftDelete
    {
        protected Base()
        {

        }

        protected Base(long id, bool deleted, DateTime? deletedAt, string deletedBy)
        {
            Id = id;
            Deleted = deleted;
            DeletedAt = deletedAt;
            DeletedBy = deletedBy;
        }

        public long Id { get; protected set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string DeletedBy { get; set; }
    }
}
