using EA.Domain.Abstraction;

namespace EA.Domain.Primitives
{
    public abstract class BaseEntity<T> : IBaseEntity
    {
        public T Id { get; protected set; }

        public bool IsDeleted { get; set; }
    }
}
