using KeySkills.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace KeySkills.Core.Data.Sqlite
{
    public class SqliteDbContext : BaseDbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Keyword>()
                .Property(k => k.Name)
                .HasColumnType("TEXT COLLATE NOCASE"); // Case Insensitive Text
        }
    }
}