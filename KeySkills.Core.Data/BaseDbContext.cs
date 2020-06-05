using KeySkills.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace KeySkills.Core.Data
{
    public abstract class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options) {}

        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
    }
}