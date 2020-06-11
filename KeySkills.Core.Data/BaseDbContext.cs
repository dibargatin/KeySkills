using System.Linq;
using KeySkills.Core.Data.SeedData;
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
            CreateKeywordEntity(modelBuilder);
            SeedKeywordEntityData(modelBuilder);

            CreateVacancyEntity(modelBuilder);
            CreateVacancyKeywordEntity(modelBuilder);
        }

        private void CreateKeywordEntity(ModelBuilder modelBuilder)
        {
            var keywordEntity = modelBuilder.Entity<Keyword>();
            
            keywordEntity
                .HasIndex(k => k.Name);

            keywordEntity
                .Property(k => k.Name)
                .IsRequired();

            keywordEntity
                .Property(k => k.Pattern)
                .IsRequired();
        }

        private void SeedKeywordEntityData(ModelBuilder modelBuilder) =>
            modelBuilder.Entity<Keyword>()
                .HasData(new KeywordSeedData().Items.Select(i => i.Keyword));

        private void CreateVacancyEntity(ModelBuilder modelBuilder)
        {
            var vacancyEntity = modelBuilder.Entity<Vacancy>();
            
            vacancyEntity
                .HasIndex(v => v.Link)
                .IsUnique();

            vacancyEntity
                .Property(v => v.Link)
                .IsRequired();

            vacancyEntity
                .Property(v => v.Title)
                .IsRequired();

            vacancyEntity
                .Property(v => v.Description)
                .IsRequired();

            vacancyEntity
                .Property(v => v.PublishedAt)
                .IsRequired();

            vacancyEntity
                .Property(v => v.CountryCode)
                .HasMaxLength(2)
                .HasConversion(new EnumToStringConverter<Country>());
        }

        private void CreateVacancyKeywordEntity(ModelBuilder modelBuilder)
        {
            var vacancyKeywordEntity =  modelBuilder.Entity<VacancyKeyword>();

            vacancyKeywordEntity
                .HasKey(vk => new { vk.VacancyId, vk.KeywordId });

            vacancyKeywordEntity
                .HasOne(vk => vk.Vacancy)
                .WithMany(v => v.Keywords)
                .HasForeignKey(vk => vk.VacancyId);

            vacancyKeywordEntity
                .HasOne(vk => vk.Keyword)
                .WithMany(k => k.Vacancies)
                .HasForeignKey(vk => vk.KeywordId);
        }
    }
}