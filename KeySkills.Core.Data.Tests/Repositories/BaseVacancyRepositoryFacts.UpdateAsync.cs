using System;
using System.Linq;
using FluentAssertions;
using KeySkills.Core.Data.Repositories;
using KeySkills.Core.Models;

namespace KeySkills.Core.Data.Tests
{
    public abstract partial class BaseVacancyRepositoryFacts<TFixture, TContext>
        where TFixture : BaseDbFixture<TContext>
        where TContext : BaseDbContext
    {
        public async void UpdateAsync_ShouldUpdateVacancy()
        {
            using (var transaction = _fixture.Connection.BeginTransaction())
            {
                Vacancy entity;

                using (var context = _fixture.CreateContext(transaction))
                {
                    var repository = new VacancyRepository(context);

                    entity = (await repository.GetAsync(v => v.Keywords.Count() > 0)).First();

                    entity.Link = Guid.NewGuid().ToString();
                    entity.Title = Guid.NewGuid().ToString();
                    entity.Description = Guid.NewGuid().ToString();
                    entity.CountryCode = Country.ZW;
                    entity.Keywords = new[] {
                        new VacancyKeyword {
                            KeywordId = 1
                        },
                        new VacancyKeyword {
                            KeywordId = 3
                        },
                    };

                    await repository.UpdateAsync(entity);
                }

                using (var context = _fixture.CreateContext(transaction))
                {
                    var repository = new VacancyRepository(context);

                    var vacancies = await repository.GetAsync(v => v.VacancyId == entity.VacancyId);

                    vacancies.Should().HaveCount(1);
                    vacancies.Select(ExtractForComparison).First()
                        .Should().BeEquivalentTo(
                            ExtractForComparison(entity)
                        );
                }
            }
        }
    }
}