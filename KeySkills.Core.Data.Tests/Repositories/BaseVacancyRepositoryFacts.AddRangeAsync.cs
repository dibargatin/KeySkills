using System;
using System.Collections.Generic;
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
        public class InvalidVacanciesCollections : TheoryData<IEnumerable<Vacancy>>
        {
            public InvalidVacanciesCollections()
            {
                foreach (var vacancy in InvalidVacancies) 
                {
                    Add(new[] {vacancy});
                }
                Add(InvalidVacancies);
                Add(Enumerable.Concat(ValidVacancies, InvalidVacancies));
            }
        }

        public static TheoryData<IEnumerable<Vacancy>> InvalidVacanciesCollectionsData =>
            new InvalidVacanciesCollections();

        [Theory]
        [MemberData(nameof(InvalidVacanciesCollectionsData))]
        public async void AddRangeAsync_ShouldNotAddInvalidVacancies(IEnumerable<Vacancy> entities)
        {
            using var context = _fixture.CreateContext();
            var repository = new VacancyRepository(context);
            
            this.Invoking(async _ => await repository.AddRangeAsync(entities))
                .Should().Throw<Exception>();

            var vacancies = await repository.GetAllAsync();

            vacancies.Where(v => entities.Any(e => e.Link == v.Link))
                .Should().BeEmpty();
        }

        [Fact]
        public async void AddRangeAsync_ShouldAddValidVacancies()
        {
            var entities = ValidVacancies;

            using (var transaction = _fixture.Connection.BeginTransaction())
            {
                using (var context = _fixture.CreateContext(transaction))
                {
                    var repository = new VacancyRepository(context);

                    await repository.AddRangeAsync(entities);
                }

                using (var context = _fixture.CreateContext(transaction))
                {
                    var repository = new VacancyRepository(context);

                    var vacancies = await repository.GetAllAsync();

                    vacancies.Where(v => entities.Any(e => e.Link == v.Link))
                        .Select(ExtractForComparison)
                        .Should().NotBeEmpty()
                        .And.BeEquivalentTo(
                            entities.Select(ExtractForComparison)
                        );
                }
            }
        }
    }
}