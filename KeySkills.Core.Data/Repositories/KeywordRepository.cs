using KeySkills.Core.Models;
using KeySkills.Core.Repositories;

namespace KeySkills.Core.Data.Repositories
{
    public class KeywordRepository : BaseRepository<Keyword, BaseDbContext>, IKeywordRepository
    {
        public KeywordRepository(BaseDbContext context) : base(context) {}
    }
}