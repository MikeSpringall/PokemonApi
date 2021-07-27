using PokemonAPI.Domain.Models;
using System.Threading.Tasks;

namespace PokemonAPI.Domain.Services
{
    public interface IPokemonService
    {
        Task<Pokemon> GetPokemon(string name);
    }
}
