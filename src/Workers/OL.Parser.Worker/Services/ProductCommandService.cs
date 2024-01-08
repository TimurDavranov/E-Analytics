using EAnalytics.Common.Abstractions;
using EAnalytics.Common.Configurations;
using Microsoft.Extensions.Options;
using OL.Infrastructure.Commands.Product;

namespace OL.Parser.Worker.Services;

public class ProductCommandService(IOptions<AppConfig> options, IHttpClientFactory factory)
    : CustomHttpClient(options.Value.OLCommandUrl, factory)
{
    private const string controller = "product";

    public Task AddOlProductCommand(AddOlProductCommand command)
    {
        return PostNoResult($"{controller}/create", command);
    }

    public Task UpdateOlProductCommand(UpdateOlProductCommand command)
    {
        return PostNoResult($"{controller}/update", command);
    }
}
