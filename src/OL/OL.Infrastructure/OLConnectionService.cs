using EA.Application.Abstractions;
using EA.Application.Configurations;
using Microsoft.Extensions.Options;
using OL.Domain.Dtos;

namespace EA.Infrastructure.Services;

public interface IOLConnectionService
{
    Task<OLBaseResponse<OLCategoryResponse<OLCategoriesDto>>> GetCategories();
}

public class OLConnectionService : ApiClient, IOLConnectionService
{
    private readonly IOptions<AppConfig> _config;
    public OLConnectionService(IHttpClientFactory httpClientFactory, IOptions<AppConfig> config) : base(config.Value.OLBaseUrl, httpClientFactory)
    {
        _config = config;
    }

    public Task<OLBaseResponse<OLCategoryResponse<OLCategoriesDto>>> GetCategories()
    {
        return GetAsync<OLBaseResponse<OLCategoryResponse<OLCategoriesDto>>>(_config.Value.OLGetCategoriesUrl);
    }
}
