using EAnalytics.Common.Abstractions;
using EAnalytics.Common.Configurations;
using EAnalytics.Common.Primitives;
using EAnalytics.Common.Primitives.DTOs;
using EAnalytics.Common.Queries;
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
    
    public Task<GetAllResponse<CategoryResponse>> GetBySystemIds(CategoryBySystemIdsRequest request) =>
        Post<GetAllResponse<CategoryResponse>>($"{controller}/GetBySystemIds", request);

    public Task<GetAllResponse<CategoryIdsResponse>?> GetAllIds() =>
        Get<GetAllResponse<CategoryIdsResponse>>($"{controller}/GetAllIds");
}
