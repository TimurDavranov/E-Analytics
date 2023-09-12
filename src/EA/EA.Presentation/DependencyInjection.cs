using Microsoft.Extensions.DependencyInjection;

namespace EA.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection service)
        {
            return service;
        }
    }
}
