using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EAnalytics.Common.Abstractions;
using EAnalytics.Common.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OL.Infrastructure.Commands.Categories;

namespace OL.Parser.Worker.Services
{
    public class WebApiGetwayService : CustomHttpClient
    {
        public WebApiGetwayService(IOptions<AppConfig> options, IHttpClientFactory factory) : base(options.Value.WebApiGatewayUrl, factory)
        {
        }
    }
}