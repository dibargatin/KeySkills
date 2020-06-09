using System.Data.Common;
using KeySkills.Core.Data.Tests;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KeySkills.Core.Data.Sqlite.Tests
{
    public class SqliteDbFixture : BaseDbFixture<SqliteDbContext>
    {
        public SqliteDbFixture() : base(
            new SqliteConnection($"Filename={nameof(SqliteDbFixture)}.db")
        ) {}

        public override SqliteDbContext CreateContext() =>
            new SqliteDbContext(
                new DbContextOptionsBuilder<SqliteDbContext>()
                    .UseSqlite(Connection)
                    .Options
                );
    }

    [CollectionDefinition(nameof(BaseDbFixture<BaseDbContext>))]
    public class CollectionFixture : ICollectionFixture<SqliteDbFixture> {}
}