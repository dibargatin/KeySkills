using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KeySkills.Core.Data.Sqlite
{
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