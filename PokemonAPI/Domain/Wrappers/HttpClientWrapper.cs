using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonAPI.Domain.Wrappers
{
    public class HttpClientWrapper : IHttpClient
    {
        public Task<HttpResponseMessage> GetAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> PostAsync(string url, FormUrlEncodedContent content)
        {
            throw new NotImplementedException();
        }
    }
}
