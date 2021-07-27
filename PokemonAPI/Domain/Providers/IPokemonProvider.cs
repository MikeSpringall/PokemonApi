using PokemonAPI.Domain.Models;
using System.Threading.Tasks;

namespace PokemonAPI.Domain.Providers
{
    public interface IPokemonProvider
    {
        Task<Pokemon> GetPokemon(string name);

        Task<Pokemon> GetPokemonTranslated(string name);
    }
}
