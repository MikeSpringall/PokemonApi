using PokemonAPI.Domain.Models;

namespace PokemonAPI.Domain.Mappers
{
    public interface IPokemonMapper
    {
        Pokemon Map(string response);
    }
}
