using PokemonAPI.Domain.Mappers;
using PokemonAPI.Domain.Models;
using PokemonAPI.Domain.Wrappers;
using System.Threading.Tasks;

namespace PokemonAPI.Domain.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IHttpClient _httpClient;
        private readonly IPokemonMapper _pokemonMapper;

        public PokemonService(IHttpClient httpClient, IPokemonMapper pokemonMapper)
        {
            _httpClient = httpClient;
            _pokemonMapper = pokemonMapper;
        }

        public async Task<Pokemon> GetPokemon(string name)
        {
            var result = await _httpClient.GetAsync($"/api/v2/pokemon-species/{name}/");

            return _pokemonMapper.Map(await result.Content?.ReadAsStringAsync());
        }
    }
}
