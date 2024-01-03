using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EAnalytics.Common.Converters;

namespace EAnalytics.Common.Abstractions
{
    public abstract class CustomHttpClient : IDisposable
    {
        private readonly IHttpClientFactory? _factory;
        private readonly string BaseUrl;
        private const string jsonResponseKey = "application/json";

        protected CustomHttpClient(string baseUrl)
        {
            this.BaseUrl = baseUrl;
        }

        protected CustomHttpClient(string baseUrl, IHttpClientFactory factory)
        {
            _factory = factory;
            this.BaseUrl = baseUrl;
        }


        protected HttpClient CreateHttpClient(string? token = null)
        {
            HttpClient client;

            if (_factory is null)
            {
                client = new HttpClient()
                {
                    BaseAddress = new Uri(BaseUrl)
                };
            }
            else
            {

                client = _factory.CreateClient();
                client.BaseAddress = new Uri(BaseUrl);
            }

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(jsonResponseKey));
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        private static async Task<T?> ConvertResponse<T>(HttpResponseMessage message) where T : class
        {
            if (!message.IsSuccessStatusCode)
                throw new HttpRequestException(message.ReasonPhrase);
            var response = await message.Content.ReadFromJsonAsync<T>(CustomJsonSerializerOptions._options);

            return response;
        }

        protected virtual async Task<T?> Post<T>(string route, object body, string? token = null) where T : class
        {
            var client = CreateHttpClient(token);
            var serialized = JsonSerializer.Serialize(body);
            var content = new StringContent(serialized, Encoding.UTF8, jsonResponseKey);
            var request = await client.PostAsync(route, content);
            return await ConvertResponse<T>(request);
        }

        protected virtual async Task<T?> Get<T>(string route, string? token = null) where T : class
        {
            var client = CreateHttpClient(token);
            var request = await client.GetAsync(route);
            return await ConvertResponse<T>(request);
        }

        public void Dispose()
        {
        }
    }
}