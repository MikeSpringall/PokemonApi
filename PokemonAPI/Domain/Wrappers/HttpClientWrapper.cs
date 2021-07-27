using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonAPI.Domain.Wrappers
{
    public class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper(string baseUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            return _httpClient.GetAsync(url);
        }

        public Task<HttpResponseMessage> PostAsync(string url, FormUrlEncodedContent content)
        {
            return _httpClient.PostAsync(url, content);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
