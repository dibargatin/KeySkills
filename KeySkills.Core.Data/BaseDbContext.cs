using KeySkills.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KeySkills.Core.Data
{
    public abstract class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options) {}

        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<VacancyKeyword> VacancyKeywords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Keyword 
            modelBuilder.Entity<Keyword>()
                .Property(k => k.Name)
                .IsRequired();

            modelBuilder.Entity<Keyword>()
                .Property(k => k.Pattern)
                .IsRequired();

            // Vacancy
            modelBuilder.Entity<Vacancy>()
                .HasIndex(v => v.Link)
                .IsUnique();

            modelBuilder.Entity<Vacancy>()
                .Property(v => v.Link)
                .IsRequired();

            modelBuilder.Entity<Vacancy>()
                .Property(v => v.Title)
                .IsRequired();

            modelBuilder.Entity<Vacancy>()
                .Property(v => v.Description)
                .IsRequired();

            modelBuilder.Entity<Vacancy>()
                .Property(v => v.PublishedAt)
                .IsRequired();

            modelBuilder.Entity<Vacancy>()
                .Property(v => v.CountryCode)
                .HasMaxLength(2)
                .HasConversion(new EnumToStringConverter<Country>());

            // VacancyKeyword
            modelBuilder.Entity<VacancyKeyword>()
                .HasKey(vk => new { vk.VacancyId, vk.KeywordId });

            modelBuilder.Entity<VacancyKeyword>()
                .HasOne(vk => vk.Vacancy)
                .WithMany(v => v.Keywords)
                .HasForeignKey(vk => vk.VacancyId);

            modelBuilder.Entity<VacancyKeyword>()
                .HasOne(vk => vk.Keyword)
                .WithMany(k => k.Vacancies)
                .HasForeignKey(vk => vk.KeywordId);
        }
    }
}