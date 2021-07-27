using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokemonAPI.Domain.Models;
using PokemonAPI.Domain.Providers;
using PokemonAPI.Domain.Services;
using Shouldly;
using System.Threading.Tasks;

namespace PokemonAPI.Tests.Unit.Providers
{
    [TestClass]
    public class PokemonProviderTests
    {
        private Mock<IPokemonService> _pokemonService;
        private Mock<ITranslationService> _translationService;
        private PokemonProvider _pokemonProvider;

        [TestInitialize]
        public void Setup()
        {
            _pokemonService = new Mock<IPokemonService>();
            _translationService = new Mock<ITranslationService>();
            _pokemonProvider = new PokemonProvider(_pokemonService.Object, _translationService.Object);
        }

        [TestMethod]
        public async Task GetPokemon_should_call_pokemonservice_with_expected_name()
        {
            var pokemonName = "pokemon";

            await _pokemonProvider.GetPokemon(pokemonName);

            _pokemonService.Verify(x => x.GetPokemon(pokemonName), Times.Once);
        }

        [TestMethod]
        public async Task GetPokemon_should_return_pokemon_from_pokemonservice()
        {
            var pokemonName = "pokemon";
            var expectedPokemon = new Pokemon(pokemonName, "pokemon description", "pokemon habitat", true);
            _pokemonService.Setup(x => x.GetPokemon(pokemonName)).Returns(Task.FromResult(expectedPokemon));

            var pokemon = await _pokemonProvider.GetPokemon(pokemonName);

            pokemon.ShouldBe(expectedPokemon);
        }

        [TestMethod]
        public async Task GetPokemonTranslated_should_return_null_if_pokemonservice_returns_null()
        {
            _pokemonService.Setup(x => x.GetPokemon(It.IsAny<string>())).Returns(Task.FromResult<Pokemon>(null));

            var pokemon = await _pokemonProvider.GetPokemonTranslated("pokemon name");

            pokemon.ShouldBeNull();
        }

        [TestMethod]
        public async Task GetPokemonTranslated_should_call_translationservice_with_default_of_shakespeare_and_set_expected_translated_description()
        {
            var pokemonName = "pokemon";
            var expectedTranslatedDescription = "pokemon description shakespeare";
            var originalPokemon = new Pokemon(pokemonName, "pokemon description", "pokemon habitat", false);
            var expectedPokemon = originalPokemon with { Description = expectedTranslatedDescription };
            _pokemonService.Setup(x => x.GetPokemon(It.IsAny<string>())).Returns(Task.FromResult(originalPokemon));
            _translationService.Setup(x => x.GetTranslation(TranslationType.Shakespeare, originalPokemon.Description)).Returns(Task.FromResult(expectedTranslatedDescription));

            var pokemon = await _pokemonProvider.GetPokemonTranslated(pokemonName);

            pokemon.ShouldBe(expectedPokemon);
            _translationService.Verify(x => x.GetTranslation(TranslationType.Shakespeare, originalPokemon.Description), Times.Once);
        }

        [TestMethod]
        public async Task GetPokemonTranslated_should_call_translationservice_with_yoda_type_if_islegendary_pokemon_and_set_expected_translated_description()
        {
            var pokemonName = "pokemon";
            var expectedTranslatedDescription = "pokemon description yoda";
            var originalPokemon = new Pokemon(pokemonName, "pokemon description", "pokemon habitat", true);
            var expectedPokemon = originalPokemon with { Description = expectedTranslatedDescription };
            _pokemonService.Setup(x => x.GetPokemon(It.IsAny<string>())).Returns(Task.FromResult(originalPokemon));
            _translationService.Setup(x => x.GetTranslation(TranslationType.Yoda, originalPokemon.Description)).Returns(Task.FromResult(expectedTranslatedDescription));

            var pokemon = await _pokemonProvider.GetPokemonTranslated(pokemonName);

            pokemon.ShouldBe(expectedPokemon);
            _translationService.Verify(x => x.GetTranslation(TranslationType.Yoda, originalPokemon.Description), Times.Once);
        }

        [DataTestMethod]
        [DataRow("cave")]
        [DataRow("CAVE")]
        [DataRow("CaVe")]
        public async Task GetPokemonTranslated_should_call_translationservice_with_yoda_type_if_habitat_is_cave_with_different_casing_and_set_expected_translated_description(string habitat)
        {
            var pokemonName = "pokemon";
            var expectedTranslatedDescription = "pokemon description yoda";
            var originalPokemon = new Pokemon(pokemonName, "pokemon description", habitat, false);
            var expectedPokemon = originalPokemon with { Description = expectedTranslatedDescription };
            _pokemonService.Setup(x => x.GetPokemon(It.IsAny<string>())).Returns(Task.FromResult(originalPokemon));
            _translationService.Setup(x => x.GetTranslation(TranslationType.Yoda, originalPokemon.Description)).Returns(Task.FromResult(expectedTranslatedDescription));

            var pokemon = await _pokemonProvider.GetPokemonTranslated(pokemonName);

            pokemon.ShouldBe(expectedPokemon);
            _translationService.Verify(x => x.GetTranslation(TranslationType.Yoda, originalPokemon.Description), Times.Once);
        }
    }
}
