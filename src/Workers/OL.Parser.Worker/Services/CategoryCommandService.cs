using EAnalytics.Common.Abstractions;
using EAnalytics.Common.Configurations;
using Microsoft.Extensions.Options;
using OL.Infrastructure.Commands.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OL.Parser.Worker.Services
{
    public sealed class CategoryCommandService(IOptions<AppConfig> config, IHttpClientFactory factory)
        : CustomHttpClient(config.Value.OLCommandUrl, factory)
    {
        private const string controller = "category";

        public Task AddOlCategoryCommand(AddOlCategoryCommand command)
        {
            return PostNoResult($"{controller}/create", command);
        }

        public Task UpdateOlCategoryCommand(UpdateOlCategoryCommand command)
        {
            return PostNoResult($"{controller}/update", command);
        }
    }
}
