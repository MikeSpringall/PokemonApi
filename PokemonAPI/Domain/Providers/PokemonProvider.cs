using PokemonAPI.Domain.Models;
using System;
using System.Threading.Tasks;

namespace PokemonAPI.Domain.Providers
{
    public class PokemonProvider : IPokemonProvider
    {
        public Task<Pokemon> GetPokemon(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Pokemon> GetPokemonTranslated(string name)
        {
            throw new NotImplementedException();
        }
    }
}
