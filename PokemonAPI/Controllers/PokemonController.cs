using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Domain.Models;
using PokemonAPI.Domain.Providers;
using System.Threading.Tasks;

namespace PokemonAPI.Controllers
{
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonProvider _pokemonProvider;

        public PokemonController(IPokemonProvider pokemonProvider)
        {
            _pokemonProvider = pokemonProvider;
        }

        [HttpGet("pokemon/{name}")]
        public async Task<Pokemon> Get(string name)
        {
            return await _pokemonProvider.GetPokemon(name.ToLower());
        }

        [HttpGet("pokemon/translated/{name}")]
        public async Task<Pokemon> GetTranslated(string name)
        {
            return await _pokemonProvider.GetPokemonTranslated(name.ToLower());
        }
    }
}
