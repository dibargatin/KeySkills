using System;
using KeySkills.Core.Models;

namespace KeySkills.Crawler.Core.Clients
{
    /// <summary>
    /// Defines a provider of vacancies
    /// </summary>
    public interface IJobBoardClient
    {
        /// <summary>
        /// Gets vacancies from a job board
        /// </summary>
        /// <returns>Observable sequence of received vacancies</returns>
        IObservable<Vacancy> GetVacancies();
    }
}