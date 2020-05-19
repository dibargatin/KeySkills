using System;
using KeySkills.Crawler.Core.Models;

namespace KeySkills.Crawler.Core
{
    public interface IJobBoardClient
    {
        IObservable<Vacancy> GetVacancies();
    }
}