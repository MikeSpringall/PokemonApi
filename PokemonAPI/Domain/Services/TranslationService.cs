using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PokemonAPI.Domain.Models;
using PokemonAPI.Domain.Models.Responses;
using PokemonAPI.Domain.Wrappers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonAPI.Domain.Services
{
    public class TranslationService : ITranslationService
    {
        private IHttpClient _httpClient;
        private readonly ILogger<TranslationService> _logger;

        public TranslationService(IHttpClient httpClient, ILogger<TranslationService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetTranslation(TranslationType translationType, string description)
        {
            return await GetTranslationFor(translationType, description);
        }

        private async Task<string> GetTranslationFor(TranslationType translationType, string description)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("text", description)
            });
            var response = await _httpClient.PostAsync($"/translate/{translationType}.json", content);

            try
            {
                var translation = JsonConvert.DeserializeObject<TranslationResponse>(await response.Content?.ReadAsStringAsync());
                
                if (translation?.Success != null)
                {
                    if (translation.Success.Total > 0)
                    {
                        if (!string.IsNullOrEmpty(translation?.Contents?.Translated))
                        {
                            description = translation?.Contents?.Translated;
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"Description was not translated. No successful translations. TranslationType: {translationType}. Description: {description}");
                    }
                }
                else
                {
                    _logger.LogInformation($"Description was not translated. TranslationType: {translationType}. Description: {description}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Deserializing translation response.");
            }

            return description;
        }
    }
}
