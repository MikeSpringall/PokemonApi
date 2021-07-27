using Microsoft.Extensions.Logging;
using PokemonAPI.Domain.Models;
using PokemonAPI.Domain.Services;
using System.Threading.Tasks;

namespace PokemonAPI.Domain.Providers
{
    public class PokemonProvider : IPokemonProvider
    {
        private readonly IPokemonService _pokemonService;
        private readonly ITranslationService _translationService;
        private readonly ILogger<PokemonProvider> _logger;

        public PokemonProvider(IPokemonService pokemonService, ITranslationService translationService)
        {
            _pokemonService = pokemonService;
            _translationService = translationService;
        }
        public async Task<Pokemon> GetPokemon(string name)
        {
            return await _pokemonService.GetPokemon(name);
        }

        public async Task<Pokemon> GetPokemonTranslated(string name)
        {
            var pokemon = await _pokemonService.GetPokemon(name);
            if (pokemon != null)
            {
                var translationType = TranslationType.Shakespeare;
                if (PokemonHabitatIsCave(pokemon.Habitat) || pokemon.IsLegendary)
                {
                    translationType = TranslationType.Yoda;
                }
                return pokemon with { Description = await _translationService.GetTranslation(translationType, pokemon.Description) };
            }
            return pokemon;
        }

        private bool PokemonHabitatIsCave(string habitat)
        {
            return habitat.ToLower() == PokemonHabitat.Cave.ToString().ToLower();
        }
    }
}
