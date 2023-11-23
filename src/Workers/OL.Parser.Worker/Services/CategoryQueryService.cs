using EAnalytics.Common.Abstractions;
using EAnalytics.Common.Configurations;
using Microsoft.Extensions.Options;
using OL.Infrastructure.Models.Requests.Category;
using OL.Infrastructure.Models.Responses.Category;

namespace OL.Parser.Worker.Services;

public sealed class CategoryQueryService : CustomHttpClient
{
    private const string controller = "Category";
    public CategoryQueryService(IOptions<AppConfig> config, IHttpClientFactory factory) : base(config.Value.OLQueryUrl, factory)
    {
    }

    public Task<CategoryResponse?> GetBySystemId(CategoryBySystemIdRequest request) =>
        Post<CategoryResponse?>($"{controller}/GetBySystemId", request);

    public Task<CategoryResponse?> GetByName(CategoryByNameRequest request) =>
        Post<CategoryResponse?>($"{controller}/GetByName", request);
}
