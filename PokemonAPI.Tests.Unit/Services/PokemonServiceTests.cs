using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokemonAPI.Domain.Mappers;
using PokemonAPI.Domain.Models;
using PokemonAPI.Domain.Services;
using PokemonAPI.Domain.Wrappers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PokemonAPI.Tests.Unit.Services
{
    [TestClass]
    public class PokemonServiceTests
    {
        private Mock<IHttpClient> _httpClient;
        private Mock<IPokemonMapper> _pokemonMapper;
        private PokemonService _pokemonService;

        [TestInitialize]
        public void Setup()
        {
            _httpClient = new Mock<IHttpClient>();
            _pokemonMapper = new Mock<IPokemonMapper>();
            _pokemonService = new PokemonService(_httpClient.Object, _pokemonMapper.Object);
        }

        [TestMethod]
        public async Task GetPokemon_should_make_expected_request()
        {
            var pokemonName = "pokemon";
            var expectedUrl = $"/api/v2/pokemon-species/{pokemonName}/";

            _httpClient.Setup(x => x.GetAsync(expectedUrl)).Returns(Task.FromResult(new HttpResponseMessage()));

            await _pokemonService.GetPokemon(pokemonName);

            _httpClient.Verify(x => x.GetAsync(expectedUrl), Times.Once);
        }

        [TestMethod]
        public async Task GetPokemon_should_handle_empty_response()
        {
            _httpClient.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(new HttpResponseMessage()));

            await _pokemonService.GetPokemon("pokemon name");

            _httpClient.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
            _pokemonMapper.Verify(x => x.Map(string.Empty), Times.Once);
        }

        [TestMethod]
        public async Task GetPokemon_should_return_mapped_pokemon()
        {
            var pokemonName = "pokemon name";
            var expectedUrl = $"/api/v2/pokemon-species/{pokemonName}/";
            var expectedPokemon = new Pokemon(pokemonName, "pokemon description", "pokemon habitat", true);
            var dummyJsonResponse = "{ pokemon response }";
            _httpClient.Setup(x => x.GetAsync(expectedUrl)).Returns(Task.FromResult(new HttpResponseMessage { Content = new StringContent(dummyJsonResponse) }));
            _pokemonMapper.Setup(x => x.Map(dummyJsonResponse)).Returns(expectedPokemon);

            var pokemon = await _pokemonService.GetPokemon(pokemonName);

            pokemon.ShouldBe(expectedPokemon);
            _httpClient.Verify(x => x.GetAsync(expectedUrl), Times.Once);
            _pokemonMapper.Verify(x => x.Map(dummyJsonResponse), Times.Once);
        }
    }
}
