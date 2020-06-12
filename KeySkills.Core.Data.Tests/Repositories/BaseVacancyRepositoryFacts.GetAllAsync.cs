using System.Linq;
using FluentAssertions;
using KeySkills.Core.Data.Repositories;
using KeySkills.Core.Models;
using Xunit;

namespace KeySkills.Core.Data.Tests
{
    public abstract partial class BaseVacancyRepositoryFacts<TFixture, TContext>
        where TFixture : BaseDbFixture<TContext>
        where TContext : BaseDbContext
    {
        private Vacancy ExtractForComparison(Vacancy entity) => 
            new Vacancy {
                Link = entity.Link,
                Title = entity.Title,
                Description = entity.Description,
                PublishedAt = entity.PublishedAt,
                CountryCode = entity.CountryCode,
                Keywords = entity.Keywords.Select(kv => new VacancyKeyword {
                    KeywordId = kv.KeywordId
                })
            };

        [Fact]
        public async void GetAllAsync_ShouldReturnAllSeedData()
        {
            using var context = _fixture.CreateContext();
            var repository = new VacancyRepository(context);

            var vacancies = await repository.GetAllAsync();

            vacancies.Select(ExtractForComparison)
                .Should().NotBeEmpty()
                .And.BeEquivalentTo(_fixture.VacancySeedData);
        }
    }
}