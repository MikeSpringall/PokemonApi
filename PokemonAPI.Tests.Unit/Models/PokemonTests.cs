using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonAPI.Domain.Models;
using Shouldly;

namespace PokemonAPI.Tests.Unit.Models
{
    [TestClass]
    public class PokemonTests
    {
        [TestMethod]
        public void Constructor_should_set_properties()
        {
            var expectedName = "pokemon";
            var expectedDescription = "pokemonDescription";
            var expectedHabitat = "pokemon habitat";
            var expectedIsLegendary = true;

            var pokemon = new Pokemon(expectedName, expectedDescription, expectedHabitat, expectedIsLegendary);

            pokemon.Name.ShouldBe(expectedName);
            pokemon.Description.ShouldBe(expectedDescription);
            pokemon.Habitat.ShouldBe(expectedHabitat);
            pokemon.IsLegendary.ShouldBe(expectedIsLegendary);
        }
    }
}
