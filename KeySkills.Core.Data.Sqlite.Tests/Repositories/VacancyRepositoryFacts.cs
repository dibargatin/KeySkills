using KeySkills.Core.Data.Tests;

namespace KeySkills.Core.Data.Sqlite.Tests
{
    public class VacancyRepositoryFacts : BaseVacancyRepositoryFacts<SqliteDbFixture, SqliteDbContext>
    {
        public VacancyRepositoryFacts(SqliteDbFixture fixture) : base(fixture) {}
    }
}