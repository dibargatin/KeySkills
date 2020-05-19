using Xunit;
using FluentAssertions;
using KeySkills.Crawler.Core.Helpers;
using KeySkills.Crawler.Core.Models;

namespace KeySkills.Crawler.Core.Tests
{
    public class CountryHelperFacts
    {
        public class TryGetCountryByName_Should
        {
            [Theory]
            [InlineData(null)]
            [InlineData("xyz")]
            public void ReturnNullForUnknownName(string name) =>
                CountryHelper.TryGetCountryByName(name)
                    .Should().BeNull($"because {name} is unknown name");

            [Theory]
            [InlineData("Russia", Country.RU)]
            [InlineData("Россия", Country.RU)]
            public void ReturnCountryForKnownName(string name, Country expected) =>
                CountryHelper.TryGetCountryByName(name)
                    .Should().Be(expected, $"because {name} is the name for {expected}");
        }
    }
}
