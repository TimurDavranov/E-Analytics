namespace EAnalytics.Common.Configurations;

public class AppConfig
{
    public string OLBaseUrl { get; init; }
    public string OLGetCategoriesUrl { get; init; }
    public string OLProductsUrl { get; init; }
    public string WebApiGatewayUrl { get; init; }
    public string OLCommandUrl { get; init; }
    public string EACommandUrl { get; init; }
    public string ExchangeKey { get; init; }
    public string RouteKey { get; init; }
    public string QueueKey { get; init; }
    public string OLGetProductsUrl(long categoryId) => OLProductsUrl + categoryId;
}
