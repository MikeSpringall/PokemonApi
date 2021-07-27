using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokemonAPI.Controllers;
using PokemonAPI.Domain.Models;
using PokemonAPI.Domain.Providers;
using Shouldly;
using System.Threading.Tasks;

namespace PokemonAPI.Tests.Unit
{
    [TestClass]
    public class PokemonControllerTests
    {
        private Mock<IPokemonProvider> _pokemonProvider;
        private PokemonController _pokemonController;

        [TestInitialize]

        public void Setup()
        {
            _pokemonProvider = new Mock<IPokemonProvider>();
            _pokemonController = new PokemonController(_pokemonProvider.Object);
        }

        [TestMethod]
        public async Task Get_should_call_provider_and_return_pokemon()
        {
            var pokemonName = "pokemon";
            var pokemon = new Pokemon(pokemonName, "pokemon description", "pokemon habitat", true);
            _pokemonProvider.Setup(x => x.GetPokemon(pokemonName)).Returns(Task.FromResult(pokemon));

            var result = await _pokemonController.Get(pokemonName);

            result.ShouldBe(pokemon);
            _pokemonProvider.Verify(x => x.GetPokemon(pokemonName), Times.Once);
        }

        [DataTestMethod]
        [DataRow("pokemon")]
        [DataRow("POKEMON")]
        [DataRow("PoKeMoN")]
        public async Task Get_should_call_provider_with_expected_pokemon_name(string pokemonName)
        {
            await _pokemonController.Get(pokemonName);

            _pokemonProvider.Verify(x => x.GetPokemon(pokemonName.ToLower()), Times.Once);
        }

        [TestMethod]
        public async Task GetTranslated_should_call_provider_and_return_pokemon()
        {
            var pokemonName = "pokemon";
            var pokemon = new Pokemon(pokemonName, "pokemon description", "pokemon habitat", true);
            _pokemonProvider.Setup(x => x.GetPokemonTranslated(pokemonName)).Returns(Task.FromResult(pokemon));

            var result = await _pokemonController.GetTranslated(pokemonName);

            result.ShouldBe(pokemon);
            _pokemonProvider.Verify(x => x.GetPokemonTranslated(pokemonName), Times.Once);
        }

        [DataTestMethod]
        [DataRow("pokemon")]
        [DataRow("POKEMON")]
        [DataRow("PoKeMoN")]
        public async Task GetTranslated_should_call_provider_with_expected_pokemon_name(string pokemonName)
        {
            await _pokemonController.GetTranslated(pokemonName);

            _pokemonProvider.Verify(x => x.GetPokemonTranslated(pokemonName.ToLower()), Times.Once);
        }
    }
}
