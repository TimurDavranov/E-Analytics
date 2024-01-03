using EAnalytics.Common.Primitives;

namespace EAnalytics.Common.Abstractions;

public abstract class InternalHttpClient : CustomHttpClient
{
    protected InternalHttpClient(string baseUrl) : base(baseUrl)
    {
    }

    protected InternalHttpClient(string baseUrl, IHttpClientFactory factory) : base(baseUrl, factory)
    {
    }

    protected override async Task<T?> Post<T>(string route, object body, string? token = null) where T : class
    {
        var response = await base.Post<BaseApiResponse<T>>(route, body, token);
        CheckResponse(response);
        return response!.Data;
    }

    protected override async Task<T?> Get<T>(string route, string? token = null) where T : class
    {
        var response = await base.Get<BaseApiResponse<T>>(route, token);
        CheckResponse(response);
        return response!.Data;
    }

    private void CheckResponse<T>(BaseApiResponse<T>? response) where T : class
    {
        if (response is null)
            throw new ArgumentNullException("Response is null or empty");
            
        if (!response.Success)
            throw new ArgumentException(response.Message);
    }
}