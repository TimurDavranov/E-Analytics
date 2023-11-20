namespace EAnalytics.Common.Queries
{
    public interface IQueryDispatcher
    {
        void RegisterHandler<TRequest, TResponse>(Func<TRequest, Task<TResponse>> handler) where TRequest : BaseQuery
            where TResponse : BaseQueryResponse;
        Task<BaseQueryResponse> SendAsync(BaseQuery command) ;
    }
}
