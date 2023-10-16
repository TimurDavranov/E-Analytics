namespace EAnalytics.Common.Entities
{
    public abstract class BaseEntity<T> : IBaseEntity
    {
        public T Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
