using EAnalytics.Common.Abstractions;
using EAnalytics.Common.Configurations;
using Microsoft.Extensions.Options;
using OL.Infrastructure.Commands.Categories;

namespace Web.ApiGateway.Services
{
    public class OLCategoryService : CustomHttpClient
    {
        private const string categoryController = "category";
        public OLCategoryService(IOptions<AppConfig> options, IHttpClientFactory factory) : base(options.Value.OLCommandUrl, factory)
        {
        }

        public Task AddOLCategoryCommand(AddOlCategoryCommand command)
        {
            return Post<string>($"{categoryController}/HandleCreate", command);
        }
    }
}