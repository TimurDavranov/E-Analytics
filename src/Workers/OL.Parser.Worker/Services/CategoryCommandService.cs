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
    public sealed class CategoryCommandService : CustomHttpClient
    {
        private const string controller = "category";

        public CategoryCommandService(IOptions<AppConfig> config, IHttpClientFactory factory) : base(config.Value.OLCommandUrl, factory)
        {
        }

        public Task AddOLCategoryCommand(AddOLCategoryCommand command)
        {
            return Post<string>($"{controller}/create", command);
        }
    }
}
