using System.Linq;
using FluentAssertions;
using KeySkills.Core.Data.Repositories;
using KeySkills.Core.Data.SeedData;
using Xunit;

namespace KeySkills.Core.Data.Tests
{
    public abstract partial class BaseKeywordRepositoryFacts<TFixture, TContext>
        where TFixture : BaseDbFixture<TContext>
        where TContext : BaseDbContext
    {
        [Fact]
        public async void GetAllAsync_ShouldReturnAllSeedData()
        {
            using var context = _fixture.CreateContext();
            var repository = new KeywordRepository(context);

            (await repository.GetAllAsync())
                .Should().NotBeEmpty()
                .And.HaveCount(new KeywordSeedData().Items.Count());
                /* .And.Contain(
                    new KeywordSeedData().Items.Select(item => item.Keyword)
                ); */
        }
    }
}