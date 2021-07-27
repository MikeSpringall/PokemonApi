using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PokemonAPI.Domain.Models;
using PokemonAPI.Domain.Models.Responses;
using System;
using System.Linq;

namespace PokemonAPI.Domain.Mappers
{
    public class PokemonMapper : IPokemonMapper
    {
        private readonly ILogger<PokemonMapper> _logger;

        public PokemonMapper(ILogger<PokemonMapper> logger)
        {
            _logger = logger;
        }
        public Pokemon Map(string response)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<PokemonResponse>(response);
                var description = result.Descriptions?.FirstOrDefault(x => x.Language.Name == "en")?.Description ?? string.Empty;

                return new Pokemon(result.Name, CleanDescription(description), result.Habitat.Name, result.IsLegendary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error mapping pokemon for response: {response}.");
                return null;
            }
        }

        private string CleanDescription(string description)
        {
            return description.Replace("\b", " ")
                              .Replace("\f", " ")
                              .Replace("\n", " ")
                              .Replace("\r", " ")
                              .Replace("\t", " ")
                              .Replace("  ", " ");
        }
    }
}
