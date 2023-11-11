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

    public Task<OLSystemRoot<OLSystemData<OLSystemCategory>>?> GetCategories()
    {
        return Get<OLSystemRoot<OLSystemData<OLSystemCategory>>>($"{_config.OLGetCategoriesUrl}");
    }
}