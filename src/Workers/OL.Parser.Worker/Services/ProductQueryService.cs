﻿using EAnalytics.Common.Abstractions;
using EAnalytics.Common.Configurations;
using Microsoft.Extensions.Options;
using OL.Infrastructure.Models.Requests.Product;
using OL.Infrastructure.Models.Responses.Product;

namespace OL.Parser.Worker.Services;

public class ProductQueryService(IOptions<AppConfig> config, IHttpClientFactory factory)
    : InternalHttpClient(config.Value.OLQueryUrl, factory)
{
    private const string controller = "Product";
    
    public Task<ProductResponse?> GetBySystemId(ProductBySystemIdRequest request) =>
        Post<ProductResponse>($"{controller}/GetBySystemId", request);
}
