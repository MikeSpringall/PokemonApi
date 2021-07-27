using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonAPI.Domain.Wrappers
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, FormUrlEncodedContent content);
    }
}
