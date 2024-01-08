using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace EAnalytics.Common.Helpers;

public static class SerilogHelper
{
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((host, config) =>
        {
            config.ReadFrom.Configuration(host.Configuration);
            config.Enrich.WithProperty("Application", host.HostingEnvironment.ApplicationName);
            config.WriteTo.File($"Logs/{host.HostingEnvironment.ApplicationName}.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true);
        });

        return builder;
    }

    public static ILoggingBuilder ConfigureSerilog(this ILoggingBuilder builder, IConfiguration configuration, string applicationName)
    {
        builder.ClearProviders();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithProperty("Application", applicationName)
            .WriteTo.File($"Logs/{applicationName}.log", rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true)
            .CreateLogger();
        builder.AddSerilog();
        return builder;
    }
}
