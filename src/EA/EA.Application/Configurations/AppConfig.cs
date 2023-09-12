namespace EA.Application.Configurations;

public class AppConfig
{
    public string OLBaseUrl { get; init; }
    public string OLGetCategoriesUrl { get; init; }
    public string OLProductsUrl { get; init; }
    public string OLGetProductsUrl(long categoryId) => OLProductsUrl + categoryId;
}
