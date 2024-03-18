using EAnalytics.Common.Primitives;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace EAnalytics.Common.Handlers
{
    public class GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger, RequestDelegate next)
    {
        public Task Invoke(HttpContext context)
        {
            try
            {
                return next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.InnerException?.Message ?? ex.Message);
                return HandleExeptionAsync(context, ex);
            }
        }

        public static Task HandleExeptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsJsonAsync(Details(ex.InnerException?.Message ?? ex.Message));
        }

        public static string Details(string message) =>
            JsonSerializer.Serialize(new BaseApiResponse<object>(null, false, message));
    }
}
