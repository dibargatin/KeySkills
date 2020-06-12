using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using FluentAssertions.Extensions;
using KeySkills.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace KeySkills.Core.Data.Tests
{   
    public abstract class BaseDbFixture<TContext> : IDisposable
        where TContext : BaseDbContext
    {
        public BaseDbFixture(DbConnection connection)
        {
            Connection = connection;

            SeedData();

            Connection.Open();
        }

        public DbConnection Connection { get; }
        
        public abstract TContext CreateContext();

        public TContext CreateContext(DbTransaction transaction = null)
        {
            var context = CreateContext();

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }
            
            return context;
        }

        private static readonly object _lock = new object(); 
        private static bool _databaseInitialized; 

        protected void SeedData()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.Migrate();

                        context.Vacancies.AddRange(VacancySeedData);
                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public void Dispose() => Connection.Dispose();

        public IEnumerable<Vacancy> VacancySeedData => new[] {
            new Vacancy {
                Link = "http://example.com/vacancies/1",
                Title = "Vacancy without keywords",
                Description = "Lorem Ipsum",
                PublishedAt = 1.March(2020).At(10, 20).AsUtc(),
                CountryCode = Country.US,
                Keywords = Enumerable.Empty<VacancyKeyword>()
            },
            new Vacancy {
                Link = "http://example.com/vacancies/2",
                Title = "Vacancy with one keyword",
                Description = "Lorem Ipsum",
                PublishedAt = 2.March(2020).At(10, 20).AsUtc(),
                CountryCode = Country.CA,
                Keywords = new[] {
                    new VacancyKeyword {
                        KeywordId = 1
                    }
                }
            },
            new Vacancy {
                Link = "http://example.com/vacancies/3",
                Title = "Vacancy with two keywords",
                Description = "Lorem Ipsum",
                PublishedAt = 3.March(2020).At(10, 20).AsUtc(),
                CountryCode = Country.NL,
                Keywords = new[] {
                    new VacancyKeyword {
                        KeywordId = 2
                    },
                    new VacancyKeyword {
                        KeywordId = 3
                    }
                }
            }
        };
    }
}