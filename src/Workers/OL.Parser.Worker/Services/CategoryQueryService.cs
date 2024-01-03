using EAnalytics.Common.Abstractions;
using EAnalytics.Common.Configurations;
using Microsoft.Extensions.Options;
using OL.Infrastructure.Models.Requests.Category;
using OL.Infrastructure.Models.Responses.Category;

namespace OL.Parser.Worker.Services;

public sealed class CategoryQueryService(IOptions<AppConfig> config, IHttpClientFactory factory)
    : InternalHttpClient(config.Value.OLQueryUrl, factory)
{
    private const string controller = "Category";

    public Task<CategoryResponse?> GetBySystemId(CategoryBySystemIdRequest request) =>
        Post<CategoryResponse>($"{controller}/GetBySystemId", request);

    public Task<CategoryResponse?> GetByName(CategoryByNameRequest request) =>
        Post<CategoryResponse>($"{controller}/GetByName", request);
}
