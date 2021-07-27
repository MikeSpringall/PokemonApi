using PokemonAPI.Domain.Models;
using System.Threading.Tasks;

namespace PokemonAPI.Domain.Services
{
    public interface ITranslationService
    {
        Task<string> GetTranslation(TranslationType translationType, string description);
    }
}
