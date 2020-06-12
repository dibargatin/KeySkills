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
        [Fact]
        public async void DeleteAsync()
        {
            using (var transaction = _fixture.Connection.BeginTransaction())
            {
                Vacancy entity;

                using (var context = _fixture.CreateContext(transaction))
                {
                    var repository = new VacancyRepository(context);

                    entity = (await repository.GetAsync(v => v.Keywords.Count() > 0)).First();

                    await repository.DeleteAsync(entity);
                }

                using (var context = _fixture.CreateContext(transaction))
                {
                    var repository = new VacancyRepository(context);

                    var vacancies = await repository.GetAsync(v => v.VacancyId == entity.VacancyId);

                    vacancies.Should().BeEmpty();

                    context.VacancyKeywords
                        .Where(vk => vk.VacancyId == entity.VacancyId)
                        .Should().BeEmpty();
                }
            }
        }
    }
}