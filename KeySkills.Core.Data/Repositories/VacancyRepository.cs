using KeySkills.Core.Models;
using KeySkills.Core.Repositories;

namespace KeySkills.Core.Data.Repositories
{
    public class VacancyRepository : BaseRepository<Vacancy, BaseDbContext>, IVacancyRepository
    {
        public VacancyRepository(BaseDbContext context) : base(context) {}
    }
}