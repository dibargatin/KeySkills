using System.Linq;
using FluentAssertions;
using KeySkills.Core.Data.Repositories;
using KeySkills.Core.Models;
using Xunit;

namespace KeySkills.Core.Data.Tests
{
    public abstract partial class BaseKeywordRepositoryFacts<TFixture, TContext>
        where TFixture : BaseDbFixture<TContext>
        where TContext : BaseDbContext
    {
        [Fact]
        public async void DeleteAsync_ShouldDeleteKeyword()
        {
            var entity = new Keyword {
                KeywordId = 1
            };

            using (var transaction = _fixture.Connection.BeginTransaction())
            {
                using (var context = _fixture.CreateContext(transaction))
                {
                    var repository = new KeywordRepository(context);

                    await repository.DeleteAsync(entity);
                }

                using (var context = _fixture.CreateContext(transaction))
                {
                    var repository = new KeywordRepository(context);

                    var keywords = await repository.GetAsync(k => k.KeywordId == entity.KeywordId);

                    keywords.Should().BeEmpty();
                }
            }
        }
    }
}