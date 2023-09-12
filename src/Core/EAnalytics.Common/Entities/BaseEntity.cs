namespace EAnalytics.Common.Entities
{
    public abstract class BaseEntity<T> : IBaseEntity
    {
        public T Id { get; protected set; }

        public bool IsDeleted { get; set; }
    }
}
