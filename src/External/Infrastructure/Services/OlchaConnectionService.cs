using Application.Abstractions;
using Application.Constants;
using Domain.Abstraction.Repositories;
using Domain.DTOs.Responses.Olcha;

namespace Infrastructure.Services
{
    public class OlchaConnectionService : ApiClient, IOlchaConnectionService
    {
        private readonly AppConfig _config;
        public OlchaConnectionService(IHttpClientFactory httpClientFactory, AppConfig config) : base(config.OlchaBaseUrl, httpClientFactory)
        {
            _config = config;
        }

        public Task<OlchaBaseResponse<OlchaResponse<OlchaCategoriesDto>>> GetCategories()
        {
            return GetAsync<OlchaBaseResponse<OlchaResponse<OlchaCategoriesDto>>>(_config.OlchaGetCategoriesUrl);
        }
    }
}
