using EAnalytics.Common.Queries;

namespace EAnalytics.Common.Dispatchers;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly Dictionary<Type, Func<BaseQuery, Task<BaseQueryResponse>>> _handlers = new();

    public void RegisterHandler<TRequest, TResponse>(Func<TRequest, Task<TResponse>> handler) where TRequest : BaseQuery 
                                                                                              where TResponse : BaseQueryResponse
    {
        if (_handlers.ContainsKey(typeof(TRequest)))
        {
            throw new IndexOutOfRangeException("You cannot register the same command handler twice!");
        }

        _handlers.Add(typeof(TRequest), async x => await handler((TRequest)x));
    }

    public Task<BaseQueryResponse> SendAsync(BaseQuery command)
    {
        if (_handlers.TryGetValue(command.GetType(), out var handler))
        {
            return handler(command);
        }
        else
        {
            throw new ArgumentNullException(nameof(handler), "No command handler was registered!");
        }
    }
}
