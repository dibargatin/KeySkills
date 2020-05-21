using Xunit;
using FluentAssertions;
using KeySkills.Crawler.Core.Helpers;
using KeySkills.Crawler.Core.Models;
using System;

namespace KeySkills.Crawler.Core.Tests
{
    public class CountryHelperFacts
    {
        public class GetCountryName_Should
        {
            [Fact]
            public void ReturnEnglishNameByCode() =>
                CountryHelper.GetCountryName(Country.NL)
                    .Should().Be("Netherlands");
        }

        public class TryGetCountryByName_Should
        {
            public static TheoryData<string> LoremIpsum =>
                new TheoryData<string> {
                    { null },
                    { String.Empty },
                    { "Lorem ipsum" }
                };
                
            [Theory]
            [MemberData(nameof(LoremIpsum))]
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
