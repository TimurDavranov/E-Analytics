using Application.Extentions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Abstractions
{
    public abstract class ApiClient : IDisposable
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            MaxDepth = int.MaxValue,
            Converters =
            {
                new DateTimeConverter()
            }
        };
        private readonly string JSON_RESPONSE = "application/json";
        protected string BaseUrl { get; }

        protected ApiClient(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        protected ApiClient(string baseUrl, IHttpClientFactory httpClientFactory) : this(baseUrl)
        {
            BaseUrl = baseUrl;
            _httpClientFactory = httpClientFactory;
        }

        protected HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JSON_RESPONSE));
            return client;
        }

        protected virtual async Task<T> GetAsync<T>(string routeUrl) where T : class
        {
            var client = CreateHttpClient();
            var request = await client.GetAsync(routeUrl);

            return await ConvertResponse<T>(request);
        }

        protected virtual async Task<T> PostAsync<T>(string routeUrl, object bodyData) where T : class
        {
            var client = CreateHttpClient();

            var serialized = JsonSerializer.Serialize(bodyData);
            var content = new StringContent(serialized, Encoding.UTF8, JSON_RESPONSE);
            var request = await client.PostAsync(routeUrl, content);
            return await ConvertResponse<T>(request);

        }

        protected virtual async Task<T> PutAsync<T>(string routeUrl, object bodyData) where T : class
        {
            var client = CreateHttpClient();
            var serialized = JsonSerializer.Serialize(bodyData);
            var content = new StringContent(serialized, Encoding.UTF8,JSON_RESPONSE);
            var request = await client.PutAsync(routeUrl, content);
            return await ConvertResponse<T>(request);
        }

        protected virtual async Task<T> DeleteAsync<T>(string routeUrl) where T : class
        {
            var client = CreateHttpClient();

            var request = await client.DeleteAsync(routeUrl);

            return await ConvertResponse<T>(request);

        }

        private static async Task<T> ConvertResponse<T>(HttpResponseMessage request)
        {
            if (!request.IsSuccessStatusCode)
                throw new Exception(request.ReasonPhrase);
            var response = await request.Content.ReadFromJsonAsync<T>(_options);
            return response;
        }

        public void Dispose()
        {
            
        }
    }
}
