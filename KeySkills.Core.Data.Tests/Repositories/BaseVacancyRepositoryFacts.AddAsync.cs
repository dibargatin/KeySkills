using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Extensions;
using KeySkills.Core.Data.Repositories;
using KeySkills.Core.Models;
using Xunit;

namespace KeySkills.Core.Data.Tests
{
    public abstract partial class BaseVacancyRepositoryFacts<TFixture, TContext>
        where TFixture : BaseDbFixture<TContext>
        where TContext : BaseDbContext
    {
        public static Vacancy[] InvalidVacancies => new[] {
            // Link, Title, Description are required properties
            new Vacancy(),
            new Vacancy {
                Link = String.Empty
            },
            new Vacancy {
                Title = String.Empty
            },
            new Vacancy {
                Description = String.Empty
            },
            new Vacancy {
                Link = String.Empty,
                Title = String.Empty
            },
            new Vacancy {
                Link = String.Empty,
                Description = String.Empty
            },
            new Vacancy {
                Title = String.Empty,
                Description = String.Empty
            }
        };

        public static IEnumerable<object[]> InvalidVacanciesTheoryData =>
            InvalidVacancies.Select(k => new[] { (object)k });

        [Theory]
        [MemberData(nameof(InvalidVacanciesTheoryData))]
        public async void AddAsync_ShouldNotAddInvalidVacancy(Vacancy entity)
        {
            using var context = _fixture.CreateContext();
            var repository = new VacancyRepository(context);
            
            this.Invoking(async _ => await repository.AddAsync(entity))
                .Should().Throw<Exception>();

            var vacancies = await repository.GetAsync(v => v.Link == entity.Link);

            vacancies.Should().BeEmpty();
        }

        public static Vacancy[] ValidVacancies => new[] {
            new Vacancy {
                Link = Guid.NewGuid().ToString(),
                Title = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Keywords = Enumerable.Empty<VacancyKeyword>()
            },
            new Vacancy {
                Link = Guid.NewGuid().ToString(),
                Title = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                PublishedAt = 1.March(2020).At(5,5).AsUtc(),
                Keywords = Enumerable.Empty<VacancyKeyword>()
            },
            new Vacancy {
                Link = Guid.NewGuid().ToString(),
                Title = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                PublishedAt = 2.March(2020).At(5,5).AsUtc(),
                CountryCode = Country.RU,
                Keywords = Enumerable.Empty<VacancyKeyword>()
            },
            new Vacancy {
                Link = Guid.NewGuid().ToString(),
                Title = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                PublishedAt = 3.March(2020).At(5,5).AsUtc(),
                CountryCode = Country.DE,
                Keywords = new[] {
                    new VacancyKeyword {
                        KeywordId = 1
                    }
                }
            }
        };

        public static IEnumerable<object[]> ValidVacanciesTheoryData =>
            ValidVacancies.Select(k => new[] { (object)k });

        [Theory]
        [MemberData(nameof(ValidVacanciesTheoryData))]
        public async void AddAsync_ShouldAddValidVacancy(Vacancy entity)
        {
            using (var transaction = _fixture.Connection.BeginTransaction())
            {
                using (var context = _fixture.CreateContext(transaction))
                {
                    var repository = new VacancyRepository(context);                    
                    await repository.AddAsync(entity);
                }

                using (var context = _fixture.CreateContext(transaction))
                {
                    var repository = new VacancyRepository(context);
                    var keywords = await repository.GetAsync(v => v.Link == entity.Link);

                    keywords.Should().HaveCount(1);
                    keywords.Select(ExtractForComparison).First()
                        .Should().BeEquivalentTo(
                            ExtractForComparison(entity)
                        );
                }
            }
        }

        [Fact]
        public async void AddAsync_ShouldNotAddTwoVacanciesWithTheSameLinks()
        {
            using var context = _fixture.CreateContext();
            var repository = new VacancyRepository(context);

            var existedVacancy = (await repository.GetAsync(v => v.VacancyId == 1))
                .Select(ExtractForComparison)
                .First();

            this.Invoking(async _ => await repository.AddAsync(existedVacancy))
                .Should().Throw<Exception>();

            var vacancies = await repository.GetAsync(v => v.Link == existedVacancy.Link);

            vacancies.Should().HaveCount(1);
        }
    }
}