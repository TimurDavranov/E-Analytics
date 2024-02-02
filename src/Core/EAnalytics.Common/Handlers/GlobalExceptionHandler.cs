using EAnalytics.Common.Primitives;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EAnalytics.Common.Handlers
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            logger.LogError(exception.InnerException?.Message ?? exception.Message);

            var details = new BaseApiResponse<object>(null, false, exception.InnerException?.Message ?? exception.Message);

            await httpContext.Response.WriteAsJsonAsync(details);
            return true;
        }
    }
}
