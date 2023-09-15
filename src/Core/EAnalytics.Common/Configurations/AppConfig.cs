namespace EAnalytics.Common.Configurations;

public class AppConfig
{
    public string OLBaseUrl { get; init; }
    public string OLGetCategoriesUrl { get; init; }
    public string OLProductsUrl { get; init; }
    public string ExchangeKey { get; set; }
    public string RouteKey { get; set; }
    public string QueueKey { get; set; }
    public string OLGetProductsUrl(long categoryId) => OLProductsUrl + categoryId;
}
