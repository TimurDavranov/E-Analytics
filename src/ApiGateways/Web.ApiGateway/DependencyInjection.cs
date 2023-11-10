using Web.ApiGateway.Services;

namespace Web.ApiGateway
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDI(this IServiceCollection services)
        {
            return services
                .AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddHttpClient()
                .AddScoped<OLCategoryService>();
        }
    }
}
