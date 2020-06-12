using System;
using System.Collections.Generic;
using System.Linq;
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
        public class InvalidKeywordsCollections : TheoryData<IEnumerable<Keyword>>
        {
            public InvalidKeywordsCollections()
            {
                foreach (var keyword in InvalidKeywords) 
                {
                    Add(new[] {keyword});
                }
                Add(InvalidKeywords);
                Add(Enumerable.Concat(ValidKeywords, InvalidKeywords));
            }
        }

        public static TheoryData<IEnumerable<Keyword>> InvalidKeywordsCollectionsData =>
            new InvalidKeywordsCollections();

        [Theory]
        [MemberData(nameof(InvalidKeywordsCollectionsData))]
        public async void AddRangeAsync_ShouldNotAddInvalidKeywords(IEnumerable<Keyword> entities)
        {
            using var context = _fixture.CreateContext();
            var repository = new KeywordRepository(context);
            
            this.Invoking(async _ => await repository.AddRangeAsync(entities))
                .Should().Throw<Exception>();

            var keywords = await repository.GetAllAsync();

            keywords.Where(k => entities.Any(e => e.KeywordId == k.KeywordId))
                .Should().BeEmpty();
        }

        [Fact]
        public async void AddRangeAsync_ShouldAddValidKeywords()
        {
            var entities = ValidKeywords;

            using (var transaction = _fixture.Connection.BeginTransaction())
            {
                using (var context = _fixture.CreateContext(transaction))
                {
                    var repository = new KeywordRepository(context);

                    await repository.AddRangeAsync(entities);
                }

                using (var context = _fixture.CreateContext(transaction))
                {
                    var repository = new KeywordRepository(context);

                    var keywords = await repository.GetAsync(
                        keyword => keyword.KeywordId > KeywordSeedData.MaxId
                    );

                    keywords
                        .Should().NotBeEmpty()
                        .And.BeEquivalentTo(entities);
                }
            }
        }
    }
}