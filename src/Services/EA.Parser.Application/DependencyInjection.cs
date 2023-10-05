using EAnalytics.Common.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EA.Parser.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, HostBuilderContext builder)
    {
        return EA.Application.DependencyInjection.AddApplication(services);
    }
}
