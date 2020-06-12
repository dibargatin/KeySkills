using System;
using System.Linq;
using System.Linq.Expressions;
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
        [Fact]
        public async void GetAsync_ShouldReturnExpectedResult()
        {
            var entity = _fixture
                .VacancySeedData
                .Where(v => v.Keywords.Count() > 0)
                .First();

            using var context = _fixture.CreateContext();
            var repository = new VacancyRepository(context);

            var vacancies = await repository.GetAsync(v => v.Link == entity.Link);

            vacancies.Should().HaveCount(1);
            vacancies.Select(ExtractForComparison).First()
                .Should().BeEquivalentTo(entity);
        }

        [Fact]
        public async void GetAsync_ShouldReturnEmptyCollectionWhenPredicateIsFalse()
        {
            using var context = _fixture.CreateContext();
            var repository = new VacancyRepository(context);

            var vacancies = await repository.GetAsync(_ => false);

            vacancies.Should().BeEmpty();
        }
    }
}