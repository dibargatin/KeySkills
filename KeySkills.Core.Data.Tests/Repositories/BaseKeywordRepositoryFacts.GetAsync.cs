using System;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using KeySkills.Core.Data.Repositories;
using KeySkills.Core.Data.SeedData;
using KeySkills.Core.Models;
using Xunit;

namespace KeySkills.Core.Data.Tests
{
    public abstract partial class BaseKeywordRepositoryFacts<TFixture, TContext>
        where TFixture : BaseDbFixture<TContext>
        where TContext : BaseDbContext
    {
        public static TheoryData<Expression<Func<Keyword, bool>>> GetAsyncShouldReturnExpectedResultData =>
            new TheoryData<Expression<Func<Keyword, bool>>> {
                { keyword => keyword.KeywordId == 1 },
                { keyword => keyword.Name == ".NET" },
                { keyword => keyword.Pattern.Contains("c#") }
            };

        [Theory]
        [MemberData(nameof(GetAsyncShouldReturnExpectedResultData))]
        public async void GetAsync_ShouldReturnExpectedResult(Expression<Func<Keyword, bool>> predicate)
        {
            using var context = _fixture.CreateContext();
            var repository = new KeywordRepository(context);
            
            var expected = new KeywordSeedData().Items
                .Select(item => item.Keyword)
                .Where(predicate.Compile());

            var keywords = await repository.GetAsync(predicate);

            keywords
                .Should().NotBeEmpty()
                .And.HaveCount(expected.Count())
                .And.BeEquivalentTo(expected);
        }
    }
}