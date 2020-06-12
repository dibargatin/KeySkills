using System;
using System.Threading.Tasks;
using KeySkills.Core.Models;
using KeySkills.Core.Repositories;
using KeySkills.Core.Data.SeedData;
using System.Collections.Generic;
using System.Linq;

namespace KeySkills.Core.Data.Repositories
{
    /// <summary>
    /// Represents keyword repository
    /// </summary>
    public class KeywordRepository : BaseRepository<Keyword, BaseDbContext>, IKeywordRepository
    {
        /// <summary>
        /// Initializes keyword repository
        /// </summary>
        /// <param name="context">Instance of <see cref="BaseDbContext"/> for repository</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <see langword="null"/></exception>
        public KeywordRepository(BaseDbContext context) : base(context) {}

        private bool CheckKeywordId(Keyword entity) => 
            entity.KeywordId > KeywordSeedData.MaxId;

        private string _message = $"KeywordId should be greater than {KeywordSeedData.MaxId}";

        /// <inheritdoc/>
        public override Task<Keyword> AddAsync(Keyword entity) =>
            CheckKeywordId(entity) switch {
                true => base.AddAsync(entity),
                _ => throw new ArgumentException(_message, nameof(entity))
            };

        /// <inheritdoc/>
        public override Task<IEnumerable<Keyword>> AddRangeAsync(IEnumerable<Keyword> entities) =>
            entities.All(CheckKeywordId) switch {
                true => base.AddRangeAsync(entities),
                _ => throw new ArgumentException(_message, nameof(entities))
            };
    }
}