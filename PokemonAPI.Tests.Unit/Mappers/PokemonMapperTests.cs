using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokemonAPI.Domain.Mappers;
using PokemonAPI.Domain.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonAPI.Tests.Unit.Mappers
{
    [TestClass]
    public class PokemonMapperTests
    {
        private Mock<ILogger<PokemonMapper>> _logger;
        private PokemonMapper _pokemonMapper;

        public PokemonMapper PokemonMapper { get => _pokemonMapper; set => _pokemonMapper = value; }

        [TestInitialize]
        public void Setup()
        {
            _logger = new Mock<ILogger<PokemonMapper>>();
            PokemonMapper = new PokemonMapper(_logger.Object);
        }

        [TestMethod]
        public void Map_should_return_expected_Pokemon()
        {
            var expectedName = "pokemonName";
            var expectedDescription = new List<(string Language, string Value)> { ("en", "pokemon description") };
            var expectedHabitat = "pokemon habitat";
            var expectedIsLegendary = true;
            var response = BuildJsonResponse(expectedName, expectedDescription, expectedHabitat, expectedIsLegendary);
            var expectedPokemon = new Pokemon(expectedName, expectedDescription.First().Value, expectedHabitat, expectedIsLegendary);

            var pokemon = PokemonMapper.Map(response);

            pokemon.ShouldBe(expectedPokemon);
        }

        [TestMethod]
        public void Map_should_return_first_EN_flavor_text_description()
        {
            var expectedName = "pokemonName";
            var expectedDescription = new List<(string Language, string Value)>{ ("en", "pokemon description 1"),
                                                                                 ("en", "pokemon description 2")};
            var expectedHabitat = "pokemon habitat";
            var expectedIsLegendary = true;
            var response = BuildJsonResponse(expectedName, expectedDescription, expectedHabitat, expectedIsLegendary);
            var expectedPokemon = new Pokemon(expectedName, expectedDescription.First().Value, expectedHabitat, expectedIsLegendary);

            var pokemon = PokemonMapper.Map(response);

            pokemon.ShouldBe(expectedPokemon);
        }

        [TestMethod]
        public void Map_should_return_null_description_if_no_EN_flavor_text_descriptions_available()
        {
            var expectedName = "pokemonName";
            var expectedDescription = new List<(string Language, string Value)> { ("fr", "pokemon description 1") };
            var expectedHabitat = "pokemon habitat";
            var expectedIsLegendary = true;
            var response = BuildJsonResponse(expectedName, expectedDescription, expectedHabitat, expectedIsLegendary);
            var expectedPokemon = new Pokemon(expectedName, "", expectedHabitat, expectedIsLegendary);

            var pokemon = PokemonMapper.Map(response);

            pokemon.ShouldBe(expectedPokemon);
        }

        [TestMethod]
        public void Map_should_return_null_description_if_no_flavor_text_descriptions_available()
        {
            var expectedName = "pokemonName";
            var expectedHabitat = "pokemon habitat";
            var expectedIsLegendary = false;
            var response = BuildJsonResponse(expectedName, null, expectedHabitat, expectedIsLegendary);
            var expectedPokemon = new Pokemon(expectedName, "", expectedHabitat, expectedIsLegendary);

            var pokemon = PokemonMapper.Map(response);

            pokemon.ShouldBe(expectedPokemon);
        }

        [TestMethod]
        public void Map_should_return_cleaned_description()
        {
            var description = new List<(string Language, string Value)> { ("en", "pokemon\ndescription\fwith\rformatting\bnow\tremoved") };
            var expectedDescription = "pokemon description with formatting now removed";
            var response = BuildJsonResponse("pokemonName", description, "pokemon habitat", true);

            var pokemon = PokemonMapper.Map(response);

            pokemon.Description.ShouldBe(expectedDescription);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("invalid")]
        public void Map_should_return_null_if_deserialization_fails_and_log_error(string response)
        {
            var pokemon = PokemonMapper.Map(response);

            pokemon.ShouldBe(null);
            _logger.Verify(x => x.Log(It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                          It.IsAny<EventId>(),
                          It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == $"Error mapping pokemon for response: {response}."),
                          It.IsAny<Exception>(),
                          It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        private string BuildJsonResponse(string name, List<(string Language, string Value)> descriptions, string habitat, bool isLegendary)
        {
            var flavor_text_entries = new List<string>();
            if (descriptions != null)
            {
                foreach (var description in descriptions)
                {
                    flavor_text_entries.Add($"{{\"flavor_text\":\"{description.Value}\", " +
                                                    $"\"language\":{{\"name\":\"{description.Language}\"}}}}");
                }
            }

            return "{ " +
                   $"\"name\":\"{name}\", " +
                   $"\"is_legendary\":{isLegendary.ToString().ToLower()}, " +
                   $"\"habitat\":{{\"name\": \"{habitat}\"}}" +
                   (descriptions != null ? $", \"flavor_text_entries\":[{string.Join(',', flavor_text_entries)}]" : "") +
                "}";
        }
    }
}
