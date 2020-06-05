using Xunit;
using FluentAssertions;
using KeySkills.Core.Helpers;
using KeySkills.Core.Models;
using System;

namespace KeySkills.Core.Tests
{
    public class CountryHelperFacts
    {
        public static TheoryData<string> LoremIpsum =>
            new TheoryData<string> {
                { null },
                { String.Empty },
                { "Lorem ipsum" }
            };
            
        public class GetCountryName_Should
        {
            [Fact]
            public void ReturnEnglishNameByCode() =>
                CountryHelper.GetCountryName(Country.NL)
                    .Should().Be("Netherlands");
        }

        public class TryGetCountryByName_Should : CountryHelperFacts
        {                
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
        
        public class IsUsaState_Should : CountryHelperFacts
        {
            [Theory]
            [InlineData("FL")]
            [InlineData("CA")]
            public void ReturnTrueForUsaStateCode(string code) =>
                CountryHelper.IsUsaState(code)
                    .Should().BeTrue();

            [Theory]
            [MemberData(nameof(LoremIpsum))]
            public void ReturnFalseForAnythingElse(string anythingElse) =>
                CountryHelper.IsUsaState(anythingElse)
                    .Should().BeFalse();
        }
    }
}
