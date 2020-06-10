using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KeySkills.Core.Models;
using KeySkills.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KeySkills.Core.Data.Repositories
{
    public class VacancyRepository : BaseRepository<Vacancy, BaseDbContext>, IVacancyRepository
    {
        public VacancyRepository(BaseDbContext context) : base(context) {}

        private IQueryable<Vacancy> GetVacancies() =>
            _context.Set<Vacancy>()
                .Include(v => v.Keywords)
                .ThenInclude(vk => vk.Keyword);

        public override async Task<IEnumerable<Vacancy>> GetAllAsync() =>
            await GetVacancies().ToListAsync();

        public override async Task<IEnumerable<Vacancy>> GetAsync(Expression<Func<Vacancy, bool>> predicate) =>
            await GetVacancies().Where(predicate).ToListAsync();
    }
}