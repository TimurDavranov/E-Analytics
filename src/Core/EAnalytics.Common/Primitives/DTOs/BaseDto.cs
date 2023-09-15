namespace EAnalytics.Common.Primitives.DTOs
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
