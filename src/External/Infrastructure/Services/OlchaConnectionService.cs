using Application.Abstractions;
using Application.Configurations;
using Domain.Abstraction.Services;
using Domain.DTOs.Responses.Olcha;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class OlchaConnectionService : ApiClient, IOlchaConnectionService
    {
        private readonly IOptions<AppConfig> _config;
        public OlchaConnectionService(IHttpClientFactory httpClientFactory, IOptions<AppConfig> config) : base(config.Value.OlchaBaseUrl, httpClientFactory)
        {
            _config = config;
        }

        public Task<OlchaBaseResponse<OlchaCategoryResponse<OlchaCategoriesDto>>> GetCategories()
        {
            return GetAsync<OlchaBaseResponse<OlchaCategoryResponse<OlchaCategoriesDto>>>(_config.Value.OlchaGetCategoriesUrl);
        }
    }
}
