using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KeySkills.Core.Data.Sqlite
{
    /// <summary>
    /// SQLite DbContext factory used for creating migrations
    /// </summary>
    public class SqliteDbContextFactory : IDesignTimeDbContextFactory<SqliteDbContext>
    {
        public SqliteDbContext CreateDbContext(string[] args) =>
            new SqliteDbContext(
                new DbContextOptionsBuilder<SqliteDbContext>()
                    .UseSqlite("Data Source=KeySkills.db")
                    .Options
                );
    }
}