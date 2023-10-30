using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace EAnalytics.Common;

public static class ConfigureOptions
{
    public static IServiceCollection AddOptions<T>(this IServiceCollection service, WebApplicationBuilder builder, string name) where T : class
    {
        if (builder is null || service is null) throw new ArgumentNullException(nameof(builder));

        service.Configure<T>(builder.Configuration.GetSection(name));
        return service;
    }

    public static IServiceCollection AddOptions<T>(this IServiceCollection service, HostBuilderContext builder, string name) where T : class
    {
        if (builder is null || service is null) throw new ArgumentNullException(nameof(builder));

        service.Configure<T>(builder.Configuration.GetSection(name));
        return service;
    }
}