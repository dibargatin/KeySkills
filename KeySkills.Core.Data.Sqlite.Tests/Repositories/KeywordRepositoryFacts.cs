using KeySkills.Core.Data.Tests;

namespace KeySkills.Core.Data.Sqlite.Tests
{
    public class KeywordRepositoryFacts : BaseKeywordRepositoryFacts<SqliteDbFixture, SqliteDbContext>
    {
        public KeywordRepositoryFacts(SqliteDbFixture fixture) : base(fixture) {}
    }
}