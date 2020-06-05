using Microsoft.EntityFrameworkCore;

namespace KeySkills.Core.Data.Sqlite
{
    public class SqliteDbContext : BaseDbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) {}
    }
}