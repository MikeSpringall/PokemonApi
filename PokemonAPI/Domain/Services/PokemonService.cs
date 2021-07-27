using PokemonAPI.Domain.Models;
using System;
using System.Threading.Tasks;

namespace PokemonAPI.Domain.Services
{
    public class PokemonService : IPokemonService
    {
        public Task<Pokemon> GetPokemon(string name)
        {
            throw new NotImplementedException();
        }
    }
}
