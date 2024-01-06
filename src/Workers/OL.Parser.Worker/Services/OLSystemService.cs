using EAnalytics.Common.Abstractions;
using EAnalytics.Common.Configurations;
using Microsoft.Extensions.Options;
using OL.Domain.Dtos.Responces;

namespace OL.Parser.Worker.Services;
public class OLSystemService : CustomHttpClient
{
    private readonly AppConfig _config;
    public OLSystemService(IOptions<AppConfig> options, IHttpClientFactory factory) : base(options.Value.OLBaseUrl, factory)
    {
        _config = options.Value;
    }

    public Task<OLSystemRoot<OLSystemCategoryData<OLSystemCategory>>?> GetCategories()
    {
        return Get<OLSystemRoot<OLSystemCategoryData<OLSystemCategory>>>($"{_config.OLGetCategoriesUrl}");
    }

    public Task<OLSystemRoot<OLSystemProductData>?> GetProducts(long categoryId, int page = 1)
    {
        return Get<OLSystemRoot<OLSystemProductData>>($"{_config.OLGetProductsUrl(categoryId, page)}");
    }
}