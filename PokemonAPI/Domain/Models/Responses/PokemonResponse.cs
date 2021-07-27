using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PokemonAPI.Domain.Models.Responses
{
    [ExcludeFromCodeCoverage]
    public class PokemonResponse
    {
        public string Name { get; set; }

        [JsonProperty("flavor_text_entries")]
        public List<FlavorTextEntry> Descriptions { get; set; }
        public Habitat Habitat { get; set; }

        [JsonProperty("is_legendary")]
        public bool IsLegendary { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class FlavorTextEntry
    {
        [JsonProperty("flavor_text")]
        public string Description { get; set; }
        public Language Language { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Language
    {
        public string Name { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Habitat
    {
        public string Name { get; set; }
    }
}
