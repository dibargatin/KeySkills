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
        public static Keyword[] InvalidKeywords => new[] {
            // KeywordId should be greater than KeywordSeedData.MaxId
            new Keyword {
                KeywordId = KeywordSeedData.MaxId - 1
            },
            new Keyword {
                KeywordId = KeywordSeedData.MaxId
            },
            // Name and Pattern properties are required
            new Keyword {
                KeywordId = KeywordSeedData.MaxId + 1
            },
            new Keyword {
                KeywordId = KeywordSeedData.MaxId + 1,
                Name = String.Empty
            },
            new Keyword {
                KeywordId = KeywordSeedData.MaxId + 1,
                Pattern = String.Empty
            }
        };

        public static IEnumerable<object[]> InvalidKeywordsTheoryData =>
            InvalidKeywords.Select(k => new[] { (object)k });
        
        [Theory]
        [MemberData(nameof(InvalidKeywordsTheoryData))]
        public void AddAsync_ShouldNotAddInvalidKeyword(Keyword entity)
        {
            using var context = _fixture.CreateContext();
            var repository = new KeywordRepository(context);
            
            this.Invoking(async _ => await repository.AddAsync(entity))
                .Should().Throw<Exception>();
        }

        public static Keyword[] ValidKeywords => new[] {
            new Keyword {
                KeywordId = KeywordSeedData.MaxId + 1,
                Name = String.Empty,
                Pattern = String.Empty
            },
            new Keyword {
                KeywordId = KeywordSeedData.MaxId + 2,
                Name = Guid.NewGuid().ToString(),
                Pattern = Guid.NewGuid().ToString()
            },
        };

        public static IEnumerable<object[]> ValidKeywordsTheoryData =>
            ValidKeywords.Select(k => new[] { (object)k });

        [Theory]
        [MemberData(nameof(ValidKeywordsTheoryData))]
        public async void AddAsync_ShouldAddValidKeyword(Keyword entity)
        {
            using (var transaction = _fixture.Connection.BeginTransaction())
            {
                using (var context = _fixture.CreateContext(transaction))
                {
                    var repository = new KeywordRepository(context);                    
                    await repository.AddAsync(entity);
                }

                using (var context = _fixture.CreateContext(transaction))
                {
                    var repository = new KeywordRepository(context);
                    var keywords = await repository.GetAsync(k => k.KeywordId == entity.KeywordId);

                    keywords.Should().HaveCount(1);
                    keywords.First().Should().BeEquivalentTo(entity);
                }
            }
        }
    }
}