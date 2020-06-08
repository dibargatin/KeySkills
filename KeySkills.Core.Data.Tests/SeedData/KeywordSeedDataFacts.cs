using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using KeySkills.Core.Data.SeedData;
using Xunit;

namespace KeySkills.Core.Data.Tests
{
    public class KeywordSeedDataFacts
    {
        public class Items_Should
        {
            public class KeywordSeedDataTests : TheoryData<KeywordSeedData.Item>
            {
                public KeywordSeedDataTests() =>
                    new KeywordSeedData().Items.ToList()
                        .ForEach(item => Add(item));
            }

            [Theory]
            [ClassData(typeof(KeywordSeedDataTests))]
            public void PassTests(KeywordSeedData.Item expected)
            {
                var regex = new Regex(
                    expected.Keyword.Pattern, 
                    RegexOptions.Compiled | RegexOptions.IgnoreCase
                );

                expected.PositiveTests.Where(test => !regex.IsMatch(test))
                    .Should().BeEmpty($"because all POSITIVE tests should match to {expected.Keyword.Pattern}");

                expected.NegativeTests.Where(test => regex.IsMatch(test))
                    .Should().BeEmpty($"because all NEGATIVE tests should not match to {expected.Keyword.Pattern}");
            }

            [Fact]
            public void HaveUniqueIds() =>
                new KeywordSeedData().Items
                    .GroupBy(item => item.Keyword.KeywordId)
                    .Where(group => group.Count() > 1)
                    .SelectMany(group => group.Select(item => item.Keyword))
                    .Should().BeEmpty("because keywords ids should be unique");

            [Fact]
            public void NotHaveGapsInIds() =>                
                new KeywordSeedData().Items
                    .Zip(new KeywordSeedData().Items.Skip(1))
                    .Where(pair => pair.Second.Keyword.KeywordId - pair.First.Keyword.KeywordId != 1)
                    .SelectMany(pair => new[] { 
                        pair.First.Keyword,
                        pair.Second.Keyword
                    }).Take(2)
                    .Should().BeEmpty("because keywords ids should grow monotonically");      

            [Fact]
            public void HaveUniqueNames() =>
                new KeywordSeedData().Items
                    .GroupBy(item => item.Keyword.Name)
                    .Where(group => group.Count() > 1)
                    .SelectMany(group => group.Select(item => item.Keyword))
                    .Should().BeEmpty("because keywords names should be unique");
        }
    }
}