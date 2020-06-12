using KeySkills.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace KeySkills.Core.Data.Sqlite
{
    /// <summary>
    /// Represents SQLite DbContext
    /// </summary>
    public class SqliteDbContext : BaseDbContext
    {
        /// <inheritdoc/>
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