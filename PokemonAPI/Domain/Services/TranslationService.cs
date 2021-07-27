using PokemonAPI.Domain.Models;
using System;
using System.Threading.Tasks;

namespace PokemonAPI.Domain.Services
{
    public class TranslationService : ITranslationService
    {
        public Task<string> GetTranslation(TranslationType translationType, string description)
        {
            throw new NotImplementedException();
        }
    }
}
