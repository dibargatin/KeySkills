using System;
using System.Threading.Tasks;
using KeySkills.Core.Models;
using KeySkills.Core.Repositories;
using FluentAssertions;
using KeySkills.Core.Data.SeedData;
using System.Collections.Generic;

namespace KeySkills.Core.Data.Repositories
{
    public class KeywordRepository : BaseRepository<Keyword, BaseDbContext>, IKeywordRepository
    {
        public KeywordRepository(BaseDbContext context) : base(context) {}

        private bool KeywordIdRule(int id) => 
            id > KeywordSeedData.MaxId;

        private string _because =>
            $@"because {
                nameof(Keyword.KeywordId)
            } should be greater than {
                KeywordSeedData.MaxId
            }";

        public override Task<Keyword> AddAsync(Keyword entity)
        {
            entity.KeywordId.Should().Match(id => KeywordIdRule(id), _because);
            return base.AddAsync(entity);
        }

        public override Task<IEnumerable<Keyword>> AddRangeAsync(IEnumerable<Keyword> entities)
        {
            entities.Should().OnlyContain(e => KeywordIdRule(e.KeywordId), _because);
            return base.AddRangeAsync(entities);
        }
    }
}