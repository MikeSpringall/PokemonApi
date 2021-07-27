using System.Diagnostics.CodeAnalysis;

namespace PokemonAPI.Domain.Models.Responses
{
    [ExcludeFromCodeCoverage]
    public class TranslationResponse
    {
        public Success Success { get; set; }
        public Contents Contents { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Success
    {
        public int Total { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Contents
    {
        public string Translated { get; set; }
    }
}
